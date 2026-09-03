namespace Krinexa.Application.DTOs.Auth;

// [ADDED 2026-09-03] Auth DTOs — request/response contracts for OTP and login flows

public record SendOtpRequest(string Email);

public record VerifyOtpRequest(string Email, string Code);

public record RegisterTalentRequest(
    string Email,
    string OtpCode,
    string Password,
    string Name,
    string ProfileType,        // Student | Intern | Junior | Senior
    string? Mobile,
    string? PortfolioUrl,
    string? GitHubUrl,
    string? LinkedInUrl,
    string? Skills,
    // Student/Intern only
    string? College,
    string? Degree,
    string? Branch,
    string? CurrentYear,
    int? GraduationYear
);

public record RegisterClientRequest(
    string Email,
    string OtpCode,
    string Password,
    string ContactName,
    string OrganizationName,
    string? Designation,
    string? CompanySize,
    string? CompanyUrl,
    string? BusinessPhone
);

public record LoginRequest(string Email, string Password);

public record AuthResponse(
    bool Success,
    string? Token,
    string? UserId,
    string? UserType,
    string? Message
);

public record OtpSentResponse(bool Success, string Message);
