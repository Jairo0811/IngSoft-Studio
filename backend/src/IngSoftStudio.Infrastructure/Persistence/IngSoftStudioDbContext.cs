using IngSoftStudio.Domain.Projects;
using IngSoftStudio.Domain.Quality;
using IngSoftStudio.Domain.Requirements;
using IngSoftStudio.Domain.Studio;
using IngSoftStudio.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IngSoftStudio.Infrastructure.Persistence;

public sealed class IngSoftStudioDbContext(DbContextOptions<IngSoftStudioDbContext> options)
    : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>(options)
{
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<Requirement> Requirements => Set<Requirement>();
    public DbSet<Risk> Risks => Set<Risk>();
    public DbSet<TestCase> TestCases => Set<TestCase>();
    public DbSet<Defect> Defects => Set<Defect>();
    public DbSet<SimulationAttempt> SimulationAttempts => Set<SimulationAttempt>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ApplicationUser>(entity => { entity.ToTable("Users"); entity.Property(user => user.FullName).HasMaxLength(150).IsRequired(); });
        builder.Entity<IdentityRole<Guid>>().ToTable("Roles");
        builder.Entity<IdentityUserRole<Guid>>().ToTable("UserRoles");
        builder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims");
        builder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogins");
        builder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaims");
        builder.Entity<IdentityUserToken<Guid>>().ToTable("UserTokens");

        builder.Entity<Project>(entity =>
        {
            entity.ToTable("Projects"); entity.HasKey(project => project.Id);
            entity.Property(project => project.Name).HasMaxLength(150).IsRequired();
            entity.Property(project => project.Description).HasMaxLength(1000);
            entity.Property(project => project.Status).HasConversion<string>().HasMaxLength(30);
            entity.HasIndex(project => project.Name);
        });

        builder.Entity<Requirement>(entity =>
        {
            entity.ToTable("Requirements"); entity.HasKey(x => x.Id);
            entity.Property(x => x.Title).HasMaxLength(200).IsRequired();
            entity.Property(x => x.Description).HasMaxLength(4000).IsRequired();
            entity.Property(x => x.Type).HasConversion<string>().HasMaxLength(30);
            entity.Property(x => x.Priority).HasConversion<string>().HasMaxLength(20);
            entity.Property(x => x.Status).HasConversion<string>().HasMaxLength(30);
            entity.Property(x => x.AcceptanceCriteria).HasMaxLength(4000);
            entity.Property(x => x.Source).HasMaxLength(500);
            entity.HasIndex(x => new { x.ProjectId, x.Priority });
            entity.HasOne<Project>().WithMany().HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<Risk>(entity =>
        {
            entity.ToTable("Risks"); entity.HasKey(x => x.Id);
            entity.Property(x => x.Title).HasMaxLength(200).IsRequired();
            entity.Property(x => x.Description).HasMaxLength(2000);
            entity.Property(x => x.Mitigation).HasMaxLength(2000);
            entity.Property(x => x.Probability).HasConversion<string>().HasMaxLength(20);
            entity.Property(x => x.Impact).HasConversion<string>().HasMaxLength(20);
            entity.Property(x => x.Status).HasConversion<string>().HasMaxLength(20);
            entity.Ignore(x => x.Score);
            entity.HasIndex(x => new { x.ProjectId, x.Status });
            entity.HasOne<Project>().WithMany().HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<TestCase>(entity =>
        {
            entity.ToTable("TestCases"); entity.HasKey(x => x.Id);
            entity.Property(x => x.Title).HasMaxLength(200).IsRequired();
            entity.Property(x => x.Preconditions).HasMaxLength(2000);
            entity.Property(x => x.Steps).HasMaxLength(4000);
            entity.Property(x => x.ExpectedResult).HasMaxLength(2000);
            entity.Property(x => x.ActualResult).HasMaxLength(2000);
            entity.Property(x => x.Status).HasConversion<string>().HasMaxLength(20);
            entity.HasIndex(x => new { x.ProjectId, x.RequirementId });
            entity.HasOne<Project>().WithMany().HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.Cascade);
            entity.HasOne<Requirement>().WithMany().HasForeignKey(x => x.RequirementId).OnDelete(DeleteBehavior.NoAction);
        });

        builder.Entity<Defect>(entity =>
        {
            entity.ToTable("Defects"); entity.HasKey(x => x.Id);
            entity.Property(x => x.Title).HasMaxLength(200).IsRequired();
            entity.Property(x => x.Description).HasMaxLength(4000);
            entity.Property(x => x.Severity).HasConversion<string>().HasMaxLength(20);
            entity.Property(x => x.Priority).HasConversion<string>().HasMaxLength(20);
            entity.Property(x => x.Status).HasConversion<string>().HasMaxLength(20);
            entity.HasIndex(x => new { x.ProjectId, x.Status, x.Severity });
            entity.HasOne<Project>().WithMany().HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.Cascade);
            entity.HasOne<Requirement>().WithMany().HasForeignKey(x => x.RequirementId).OnDelete(DeleteBehavior.NoAction);
            entity.HasOne<TestCase>().WithMany().HasForeignKey(x => x.TestCaseId).OnDelete(DeleteBehavior.NoAction);
        });

        builder.Entity<SimulationAttempt>(entity =>
        {
            entity.ToTable("SimulationAttempts");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.ScenarioId).HasMaxLength(100).IsRequired();
            entity.Property(x => x.OptionId).HasMaxLength(100).IsRequired();
            entity.Property(x => x.Level).HasMaxLength(40).IsRequired();
            entity.HasIndex(x => new { x.UserId, x.CreatedAtUtc });
            entity.HasOne<ApplicationUser>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
        });
    }
}
