namespace Krinexa.Application.DTOs.Talent;

// [ADDED 2026-09-03] Talent profile DTOs

public record TalentProfileDto(
    Guid Id,
    string Name,
    string ProfileType,
    string? Summary,
    string? Availability,
    string? Location,
    string? PortfolioUrl,
    string? GitHubUrl,
    string? LinkedInUrl,
    bool IsApproved,
    List<string> Skills,
    DateTime CreatedAt
);

public record UpdateTalentProfileRequest(
    string? Name,
    string? Summary,
    string? Availability,
    string? Location,
    string? PortfolioUrl,
    string? GitHubUrl,
    string? LinkedInUrl
);

public record AddProjectRequest(
    string Name,
    string? Description,
    string? RepositoryUrl,
    string? DemoUrl,
    string? TechSummary
);

public record ProjectDto(
    Guid Id,
    string Name,
    string? Description,
    string? RepositoryUrl,
    string? DemoUrl,
    string? TechSummary,
    DateTime CreatedAt
);

// Client / Requirement DTOs
public record CreateRequirementRequest(
    string Title,
    string? Description,
    string? ExperienceLevel,
    string? Budget,
    string? Duration,
    List<string>? SkillNames
);

public record RequirementDto(
    Guid Id,
    string Title,
    string? Description,
    string? ExperienceLevel,
    string Status,
    List<string> Skills,
    DateTime CreatedAt
);

public record ExpressInterestRequest(Guid RequirementId);

public record MatchDto(
    Guid TalentProfileId,
    string Name,
    string ProfileType,
    decimal? Score,
    string? PortfolioUrl,
    string? GitHubUrl,
    List<string> Skills
);
