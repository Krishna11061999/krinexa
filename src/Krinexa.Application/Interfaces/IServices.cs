using Krinexa.Application.DTOs.Auth;
using Krinexa.Application.DTOs.Talent;

namespace Krinexa.Application.Interfaces;

// [ADDED 2026-09-03] Service interfaces — all business logic contracts

public interface IAuthService
{
    Task<OtpSentResponse> SendOtpAsync(string email);
    Task<bool> VerifyOtpAsync(string email, string code);
    Task<AuthResponse> RegisterTalentAsync(RegisterTalentRequest request);
    Task<AuthResponse> RegisterClientAsync(RegisterClientRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
}

public interface ITalentService
{
    Task<TalentProfileDto?> GetMyProfileAsync(Guid userId);
    Task<TalentProfileDto> UpdateProfileAsync(Guid userId, UpdateTalentProfileRequest request);
    Task<ProjectDto> AddProjectAsync(Guid userId, AddProjectRequest request);
    Task<List<RequirementDto>> GetOpenRequirementsAsync();
    Task ExpressInterestAsync(Guid userId, Guid requirementId);
}

public interface IClientService
{
    Task<RequirementDto> CreateRequirementAsync(Guid userId, CreateRequirementRequest request);
    Task<List<MatchDto>> GetMatchesAsync(Guid userId, Guid requirementId);
    Task<List<MatchDto>> GetInterestedCandidatesAsync(Guid userId, Guid requirementId);
}

public interface IAdminService
{
    Task<List<TalentProfileDto>> GetPendingTalentProfilesAsync();
    Task ApproveTalentAsync(Guid adminId, Guid talentProfileId);
    Task RejectTalentAsync(Guid adminId, Guid talentProfileId, string reason);
}
