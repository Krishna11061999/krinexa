using Krinexa.Domain.Enums;

namespace Krinexa.Domain.Entities;

// [ADDED 2026-09-03] Base entity with common audit fields
public abstract class BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

public class User : BaseEntity
{
    public string Email { get; set; } = string.Empty;
    public bool EmailVerified { get; set; } = false;
    public string PasswordHash { get; set; } = string.Empty;
    public UserType UserType { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation
    public TalentProfile? TalentProfile { get; set; }
    public ClientOrganization? ClientOrganization { get; set; }
    public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
}

public class OtpRecord
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Email { get; set; } = string.Empty;
    public string CodeHash { get; set; } = string.Empty;   // Hashed — never stored plain
    public DateTime ExpiresAt { get; set; }
    public DateTime? UsedAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class TalentProfile : BaseEntity
{
    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Mobile { get; set; }   // Optional — consent required before sharing
    public ProfileType ProfileType { get; set; }
    public string? Summary { get; set; }
    public string? Availability { get; set; }
    public string? Location { get; set; }
    public string? PortfolioUrl { get; set; }
    public string? GitHubUrl { get; set; }
    public string? LinkedInUrl { get; set; }
    public string? ResumeUrl { get; set; }   // Private storage path only
    public bool IsApproved { get; set; } = false;

    // Navigation
    public User User { get; set; } = null!;
    public StudentProfile? StudentProfile { get; set; }
    public ICollection<ExperienceRecord> ExperienceRecords { get; set; } = new List<ExperienceRecord>();
    public ICollection<TalentSkill> TalentSkills { get; set; } = new List<TalentSkill>();
    public ICollection<Project> Projects { get; set; } = new List<Project>();
    public ICollection<Match> Matches { get; set; } = new List<Match>();
    public ICollection<CandidateInterest> CandidateInterests { get; set; } = new List<CandidateInterest>();
}

public class StudentProfile
{
    public Guid TalentProfileId { get; set; }
    public string College { get; set; } = string.Empty;
    public string Degree { get; set; } = string.Empty;
    public string Branch { get; set; } = string.Empty;
    public string? CurrentYear { get; set; }
    public int? GraduationYear { get; set; }

    public TalentProfile TalentProfile { get; set; } = null!;
}

public class ExperienceRecord
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid TalentProfileId { get; set; }
    public string Company { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public TalentProfile TalentProfile { get; set; } = null!;
}

public class Skill
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string? Category { get; set; }

    public ICollection<TalentSkill> TalentSkills { get; set; } = new List<TalentSkill>();
    public ICollection<RequirementSkill> RequirementSkills { get; set; } = new List<RequirementSkill>();
}

public class TalentSkill
{
    public Guid TalentProfileId { get; set; }
    public Guid SkillId { get; set; }
    public short? Years { get; set; }
    public Proficiency? Proficiency { get; set; }

    public TalentProfile TalentProfile { get; set; } = null!;
    public Skill Skill { get; set; } = null!;
}

public class Project
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid TalentProfileId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? RepositoryUrl { get; set; }
    public string? DemoUrl { get; set; }
    public string? TechSummary { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public TalentProfile TalentProfile { get; set; } = null!;
}

public class ClientOrganization : BaseEntity
{
    public Guid UserId { get; set; }
    public string OrganizationName { get; set; } = string.Empty;
    public string ContactName { get; set; } = string.Empty;
    public string? Designation { get; set; }
    public string? CompanySize { get; set; }
    public string? CompanyUrl { get; set; }
    public string? BusinessPhone { get; set; }

    public User User { get; set; } = null!;
    public ICollection<ClientRequirement> Requirements { get; set; } = new List<ClientRequirement>();
}

public class ClientRequirement : BaseEntity
{
    public Guid ClientId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ExperienceLevel { get; set; }
    public string? Budget { get; set; }
    public string? Duration { get; set; }
    public RequirementStatus Status { get; set; } = RequirementStatus.Open;

    public ClientOrganization Client { get; set; } = null!;
    public ICollection<RequirementSkill> RequirementSkills { get; set; } = new List<RequirementSkill>();
    public ICollection<Match> Matches { get; set; } = new List<Match>();
    public ICollection<CandidateInterest> CandidateInterests { get; set; } = new List<CandidateInterest>();
    public ChatThread? ChatThread { get; set; }
}

public class RequirementSkill
{
    public Guid RequirementId { get; set; }
    public Guid SkillId { get; set; }
    public string? RequiredLevel { get; set; }

    public ClientRequirement Requirement { get; set; } = null!;
    public Skill Skill { get; set; } = null!;
}

public class Match : BaseEntity
{
    public Guid RequirementId { get; set; }
    public Guid TalentProfileId { get; set; }
    public decimal? Score { get; set; }
    public MatchStatus Status { get; set; } = MatchStatus.Pending;

    public ClientRequirement Requirement { get; set; } = null!;
    public TalentProfile TalentProfile { get; set; } = null!;
    public InterviewRequest? InterviewRequest { get; set; }
}

public class CandidateInterest
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid RequirementId { get; set; }
    public Guid TalentProfileId { get; set; }
    public decimal? Score { get; set; }
    public DateTime ExpressedAt { get; set; } = DateTime.UtcNow;
    public InterestStatus Status { get; set; } = InterestStatus.Pending;

    public ClientRequirement Requirement { get; set; } = null!;
    public TalentProfile TalentProfile { get; set; } = null!;
}

public class InterviewRequest : BaseEntity
{
    public Guid MatchId { get; set; }
    public DateTime? ScheduledAt { get; set; }
    public InterviewStatus Status { get; set; } = InterviewStatus.Requested;
    public string? Notes { get; set; }

    public Match Match { get; set; } = null!;
}

public class Subscription : BaseEntity
{
    public Guid UserId { get; set; }
    public string Plan { get; set; } = "MONTHLY";
    public decimal Amount { get; set; } = 10.00m;
    public bool IsActive { get; set; } = false;
    public DateOnly? StartDate { get; set; }
    public DateOnly? ExpiryDate { get; set; }
    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;

    public User User { get; set; } = null!;
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}

public class Payment
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid SubscriptionId { get; set; }
    public decimal Amount { get; set; }
    public string? ScreenshotPath { get; set; }   // Private storage ONLY — never public
    public string? PaymentReference { get; set; }
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
    public Guid? VerifiedBy { get; set; }
    public DateTime? VerifiedAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Subscription Subscription { get; set; } = null!;
}

public class ChatThread
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid RequirementId { get; set; }
    public Guid ClientId { get; set; }
    public Guid? AssignedTalentProfileId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ClientRequirement Requirement { get; set; } = null!;
    public ClientOrganization Client { get; set; } = null!;
    public TalentProfile? AssignedTalent { get; set; }
    public ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
}

public class ChatMessage
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ThreadId { get; set; }
    public Guid SenderUserId { get; set; }
    public SenderRole SenderRole { get; set; }
    public string Body { get; set; } = string.Empty;
    public string? AttachmentPath { get; set; }
    public DateTime SentAt { get; set; } = DateTime.UtcNow;

    public ChatThread Thread { get; set; } = null!;
    public User Sender { get; set; } = null!;
}

public class AdminAuditLog
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid AdminId { get; set; }
    public string Action { get; set; } = string.Empty;
    public string? Entity { get; set; }
    public Guid? EntityId { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User Admin { get; set; } = null!;
}
