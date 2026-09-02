# Database Schema

## Track A — Portfolio Website

| Entity | Important fields |
|---|---|
| Profile | Id, Name, Headline, Summary, Email, LinkedInUrl, GitHubUrl, ResumeUrl |
| Skill | Id, Name, Category, Proficiency, DisplayOrder |
| Experience | Id, Company, Role, StartDate, EndDate, Description |
| TeamMember | Id, Name, Role, Summary, ExperienceYears, Links |
| Project | Id, Name, Description, Architecture, RepositoryUrl, DemoUrl |
| Technology | Id, Name, Category |
| ProjectTechnology | ProjectId, TechnologyId |
| Article | Id, Title, Slug, Summary, Content, PublishedAt, IsPublished |

For V1, seed this data directly (no admin UI) — see `DEVELOPMENT_PLAN.md`.
Phase 2 adds JWT-authenticated CRUD for all of these.

## Track B — Marketplace Platform

| Table | Important fields |
|---|---|
| Users | Id, Email, EmailVerified, PasswordHash, UserType, CreatedAt |
| TalentProfiles | Id, UserId, Name, Mobile, ProfileType, Summary, Availability, Location |
| StudentProfiles | TalentProfileId, College, Degree, Branch, Year, GraduationYear |
| ExperienceRecords | Id, TalentProfileId, Company, Role, StartDate, EndDate, Description |
| Skills | Id, Name, Category |
| TalentSkills | TalentProfileId, SkillId, Years, Proficiency |
| Projects | Id, TalentProfileId, Name, Description, RepositoryUrl, DemoUrl, TechSummary |
| ClientOrganizations | Id, UserId, OrganizationName, ContactName |
| ClientRequirements | Id, ClientId, Title, Description, ExperienceLevel, Budget, Duration, Status |
| RequirementSkills | RequirementId, SkillId, RequiredLevel |
| Matches | Id, RequirementId, TalentProfileId, Score, Status |
| InterviewRequests | Id, MatchId, ScheduledAt, Status, Notes |
| Subscriptions | Id, UserId, Plan, Amount, IsActive, StartDate, ExpiryDate, PaymentStatus |
| Payments | Id, SubscriptionId, Amount, ScreenshotPath, PaymentReference, Status, VerifiedAt |
| Notifications | Id, UserId, Channel, Type, Status, SentAt |
| AdminAuditLogs | Id, AdminId, Action, Entity, EntityId, CreatedAt |

**Design note:** use one base `TalentProfiles` model with a `ProfileType`
discriminator plus optional profile-specific tables (e.g.
`StudentProfiles`), rather than four separate parallel systems for
student/intern/junior/experienced.

### New tables — candidate interest & project chat

| Table | Important fields |
|---|---|
| CandidateInterests | Id, RequirementId, TalentProfileId, Score, ExpressedAt, Status (Pending/Shortlisted/Declined) |
| ChatThreads | Id, RequirementId, ClientId, AssignedTalentProfileId, CreatedAt |
| ChatMessages | Id, ThreadId, SenderUserId, SenderRole (Client/KrinexaTeam/Talent), Body, AttachmentPath, SentAt |

`CandidateInterests` is additive to `Matches` — a requirement can have
rows in both, since admin-driven matching and candidate-initiated
interest use the same scoring logic but different entry points.
`ChatThreads` is one-per-requirement (see `RequirementId`), not
per-user, so all communication about a project stays in one place.

### Subscription fields (Track B) — Y/N flag alone is not enough

| Field | Example |
|---|---|
| IsActive | Y / N |
| StartDate | 2026-09-02 |
| ExpiryDate | 2026-10-02 |
| Plan | MONTHLY |
| Amount | 10.00 |
| PaymentStatus | Pending / Verified / Rejected |
| PaymentReference | Admin-entered reference |
| ScreenshotPath | Secure storage reference (never public) |
| VerifiedBy | Admin user ID |
| VerifiedAt | Timestamp |

Expiry logic: run a scheduled job to flip `IsActive` to `N` when
`CurrentDate >= ExpiryDate`, **and** re-check the same condition at
authorization time so access can't continue if the job hasn't run yet.
