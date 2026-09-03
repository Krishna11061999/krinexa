using Krinexa.Application.DTOs.Auth;
using Krinexa.Application.Interfaces;
using Krinexa.Domain.Entities;
using Krinexa.Domain.Enums;
using Krinexa.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Krinexa.Infrastructure.Services;

// [ADDED 2026-09-03] AuthService — OTP generation/verification, registration, JWT login
public class AuthService : IAuthService
{
    private readonly KrinexaDbContext _db;
    private readonly IConfiguration _config;

    public AuthService(KrinexaDbContext db, IConfiguration config)
    {
        _db = db;
        _config = config;
    }

    // --- OTP: Send ---
    public async Task<OtpSentResponse> SendOtpAsync(string email)
    {
        // Generate 6-digit cryptographically secure OTP
        var code = RandomNumberGenerator.GetInt32(100000, 999999).ToString();
        var hash = BCrypt.Net.BCrypt.HashPassword(code);

        var otp = new OtpRecord
        {
            Email = email.ToLowerInvariant().Trim(),
            CodeHash = hash,
            ExpiresAt = DateTime.UtcNow.AddMinutes(10)   // 10-minute expiry per business rules
        };
        _db.OtpRecords.Add(otp);
        await _db.SaveChangesAsync();

        // TODO: In production — send email via SendGrid/SMTP. For MVP, log to console.
        Console.WriteLine($"[Krinexa OTP] {email} → {code}");

        return new OtpSentResponse(true, $"OTP sent to {email}. Valid for 10 minutes.");
    }

    // --- OTP: Verify ---
    public async Task<bool> VerifyOtpAsync(string email, string code)
    {
        var normalizedEmail = email.ToLowerInvariant().Trim();
        var record = await _db.OtpRecords
            .Where(o => o.Email == normalizedEmail && o.UsedAt == null && o.ExpiresAt > DateTime.UtcNow)
            .OrderByDescending(o => o.CreatedAt)
            .FirstOrDefaultAsync();

        if (record == null) return false;
        if (!BCrypt.Net.BCrypt.Verify(code, record.CodeHash)) return false;

        // Invalidate immediately after successful verification
        record.UsedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return true;
    }

    // --- Register Talent ---
    public async Task<AuthResponse> RegisterTalentAsync(RegisterTalentRequest req)
    {
        // Verify OTP before creating any account
        if (!await VerifyOtpAsync(req.Email, req.OtpCode))
            return new AuthResponse(false, null, null, null, "Invalid or expired OTP.");

        if (await _db.Users.AnyAsync(u => u.Email == req.Email.ToLowerInvariant()))
            return new AuthResponse(false, null, null, null, "Email already registered.");

        var profileType = Enum.Parse<ProfileType>(req.ProfileType, ignoreCase: true);
        var userType = profileType is ProfileType.Student or ProfileType.Intern
            ? UserType.Student : UserType.Talent;

        var user = new User
        {
            Email = req.Email.ToLowerInvariant().Trim(),
            EmailVerified = true,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(req.Password),
            UserType = userType
        };
        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        var profile = new TalentProfile
        {
            UserId = user.Id,
            Name = req.Name,
            Mobile = req.Mobile,
            ProfileType = profileType,
            PortfolioUrl = req.PortfolioUrl,
            GitHubUrl = req.GitHubUrl,
            LinkedInUrl = req.LinkedInUrl
        };
        _db.TalentProfiles.Add(profile);
        await _db.SaveChangesAsync();

        // Student/Intern extra fields
        if (profileType is ProfileType.Student or ProfileType.Intern
            && req.College is not null)
        {
            _db.StudentProfiles.Add(new StudentProfile
            {
                TalentProfileId = profile.Id,
                College = req.College,
                Degree = req.Degree ?? string.Empty,
                Branch = req.Branch ?? string.Empty,
                CurrentYear = req.CurrentYear,
                GraduationYear = req.GraduationYear
            });
            await _db.SaveChangesAsync();
        }

        // Seed skills from comma-separated string
        if (!string.IsNullOrWhiteSpace(req.Skills))
            await SeedTalentSkillsAsync(profile.Id, req.Skills);

        // Create 15-day trial subscription
        _db.Subscriptions.Add(new Subscription
        {
            UserId = user.Id,
            Plan = "TRIAL",
            Amount = 0,
            IsActive = true,
            StartDate = DateOnly.FromDateTime(DateTime.UtcNow),
            ExpiryDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(15)),
            PaymentStatus = PaymentStatus.Verified
        });
        await _db.SaveChangesAsync();

        return new AuthResponse(true, GenerateJwt(user), user.Id.ToString(), user.UserType.ToString(), "Registration successful.");
    }

    // --- Register Client ---
    public async Task<AuthResponse> RegisterClientAsync(RegisterClientRequest req)
    {
        if (!await VerifyOtpAsync(req.Email, req.OtpCode))
            return new AuthResponse(false, null, null, null, "Invalid or expired OTP.");

        if (await _db.Users.AnyAsync(u => u.Email == req.Email.ToLowerInvariant()))
            return new AuthResponse(false, null, null, null, "Email already registered.");

        var user = new User
        {
            Email = req.Email.ToLowerInvariant().Trim(),
            EmailVerified = true,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(req.Password),
            UserType = UserType.Client
        };
        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        _db.ClientOrganizations.Add(new ClientOrganization
        {
            UserId = user.Id,
            OrganizationName = req.OrganizationName,
            ContactName = req.ContactName,
            Designation = req.Designation,
            CompanySize = req.CompanySize,
            CompanyUrl = req.CompanyUrl,
            BusinessPhone = req.BusinessPhone
        });
        await _db.SaveChangesAsync();

        return new AuthResponse(true, GenerateJwt(user), user.Id.ToString(), user.UserType.ToString(), "Registration successful.");
    }

    // --- Login ---
    public async Task<AuthResponse> LoginAsync(LoginRequest req)
    {
        var user = await _db.Users
            .AsNoTracking()   // Read-only query
            .FirstOrDefaultAsync(u => u.Email == req.Email.ToLowerInvariant());

        if (user == null || !user.EmailVerified)
            return new AuthResponse(false, null, null, null, "Invalid credentials.");

        if (!BCrypt.Net.BCrypt.Verify(req.Password, user.PasswordHash))
            return new AuthResponse(false, null, null, null, "Invalid credentials.");

        if (!user.IsActive)
            return new AuthResponse(false, null, null, null, "Account is inactive.");

        return new AuthResponse(true, GenerateJwt(user), user.Id.ToString(), user.UserType.ToString(), "Login successful.");
    }

    // --- JWT token generation ---
    private string GenerateJwt(User user)
    {
        var secret = _config["Jwt:Secret"] ?? throw new InvalidOperationException("JWT secret not configured.");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Role, user.UserType.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"] ?? "krinexa-api",
            audience: _config["Jwt:Audience"] ?? "krinexa-web",
            claims: claims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    // --- Helper: seed talent skills from comma-separated text ---
    private async Task SeedTalentSkillsAsync(Guid talentProfileId, string skillsCsv)
    {
        var names = skillsCsv.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        foreach (var name in names)
        {
            var skill = await _db.Skills.FirstOrDefaultAsync(s => s.Name == name)
                        ?? new Skill { Name = name };
            if (skill.Id == Guid.Empty) { _db.Skills.Add(skill); await _db.SaveChangesAsync(); }

            if (!await _db.TalentSkills.AnyAsync(ts => ts.TalentProfileId == talentProfileId && ts.SkillId == skill.Id))
                _db.TalentSkills.Add(new TalentSkill { TalentProfileId = talentProfileId, SkillId = skill.Id });
        }
        await _db.SaveChangesAsync();
    }
}
