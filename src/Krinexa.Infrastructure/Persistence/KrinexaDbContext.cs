using Krinexa.Domain.Entities;
using Krinexa.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Krinexa.Infrastructure.Persistence;

// [ADDED 2026-09-03] EF Core DbContext — maps all domain entities to PostgreSQL tables
public class KrinexaDbContext : DbContext
{
    public KrinexaDbContext(DbContextOptions<KrinexaDbContext> options) : base(options) { }

    // Auth
    public DbSet<User> Users => Set<User>();
    public DbSet<OtpRecord> OtpRecords => Set<OtpRecord>();

    // Talent
    public DbSet<TalentProfile> TalentProfiles => Set<TalentProfile>();
    public DbSet<StudentProfile> StudentProfiles => Set<StudentProfile>();
    public DbSet<ExperienceRecord> ExperienceRecords => Set<ExperienceRecord>();
    public DbSet<Skill> Skills => Set<Skill>();
    public DbSet<TalentSkill> TalentSkills => Set<TalentSkill>();
    public DbSet<Project> Projects => Set<Project>();

    // Client
    public DbSet<ClientOrganization> ClientOrganizations => Set<ClientOrganization>();
    public DbSet<ClientRequirement> ClientRequirements => Set<ClientRequirement>();
    public DbSet<RequirementSkill> RequirementSkills => Set<RequirementSkill>();

    // Matching
    public DbSet<Match> Matches => Set<Match>();
    public DbSet<CandidateInterest> CandidateInterests => Set<CandidateInterest>();
    public DbSet<InterviewRequest> InterviewRequests => Set<InterviewRequest>();

    // Subscriptions
    public DbSet<Subscription> Subscriptions => Set<Subscription>();
    public DbSet<Payment> Payments => Set<Payment>();

    // Chat
    public DbSet<ChatThread> ChatThreads => Set<ChatThread>();
    public DbSet<ChatMessage> ChatMessages => Set<ChatMessage>();

    // Admin
    public DbSet<AdminAuditLog> AdminAuditLogs => Set<AdminAuditLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // --- User ---
        modelBuilder.Entity<User>(e =>
        {
            e.HasIndex(u => u.Email).IsUnique();
            e.Property(u => u.UserType).HasConversion<string>();
        });

        // --- OtpRecord ---
        modelBuilder.Entity<OtpRecord>(e =>
        {
            e.HasIndex(o => o.Email);
        });

        // --- TalentProfile ---
        modelBuilder.Entity<TalentProfile>(e =>
        {
            e.HasIndex(t => t.UserId);
            e.Property(t => t.ProfileType).HasConversion<string>();
            e.HasOne(t => t.User).WithOne(u => u.TalentProfile)
             .HasForeignKey<TalentProfile>(t => t.UserId);
        });

        // --- StudentProfile (1-to-1 with TalentProfile) ---
        modelBuilder.Entity<StudentProfile>(e =>
        {
            e.HasKey(s => s.TalentProfileId);
            e.HasOne(s => s.TalentProfile).WithOne(t => t.StudentProfile)
             .HasForeignKey<StudentProfile>(s => s.TalentProfileId);
        });

        // --- TalentSkill (composite PK) ---
        modelBuilder.Entity<TalentSkill>(e =>
        {
            e.HasKey(ts => new { ts.TalentProfileId, ts.SkillId });
            e.Property(ts => ts.Proficiency).HasConversion<string>();
        });

        // --- RequirementSkill (composite PK) ---
        modelBuilder.Entity<RequirementSkill>(e =>
        {
            e.HasKey(rs => new { rs.RequirementId, rs.SkillId });
        });

        // --- ClientOrganization ---
        modelBuilder.Entity<ClientOrganization>(e =>
        {
            e.HasOne(c => c.User).WithOne(u => u.ClientOrganization)
             .HasForeignKey<ClientOrganization>(c => c.UserId);
        });

        // --- ClientRequirement ---
        modelBuilder.Entity<ClientRequirement>(e =>
        {
            e.Property(r => r.Status).HasConversion<string>();
        });

        // --- Match ---
        modelBuilder.Entity<Match>(e =>
        {
            e.Property(m => m.Status).HasConversion<string>();
        });

        // --- CandidateInterest (unique per pair) ---
        modelBuilder.Entity<CandidateInterest>(e =>
        {
            e.HasIndex(ci => new { ci.RequirementId, ci.TalentProfileId }).IsUnique();
            e.Property(ci => ci.Status).HasConversion<string>();
        });

        // --- InterviewRequest ---
        modelBuilder.Entity<InterviewRequest>(e =>
        {
            e.Property(ir => ir.Status).HasConversion<string>();
        });

        // --- Subscription ---
        modelBuilder.Entity<Subscription>(e =>
        {
            e.Property(s => s.PaymentStatus).HasConversion<string>();
        });

        // --- Payment ---
        modelBuilder.Entity<Payment>(e =>
        {
            e.Property(p => p.Status).HasConversion<string>();
        });

        // --- ChatMessage ---
        modelBuilder.Entity<ChatMessage>(e =>
        {
            e.HasIndex(m => m.ThreadId);
            e.Property(m => m.SenderRole).HasConversion<string>();
        });
    }
}
