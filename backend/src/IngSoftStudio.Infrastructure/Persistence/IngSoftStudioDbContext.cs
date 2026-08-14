using IngSoftStudio.Domain.Projects;
using IngSoftStudio.Domain.Requirements;
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
        builder.Entity<IdentityUserPasskey<Guid>>().ToTable("UserPasskeys");

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
            entity.HasIndex(x => new { x.ProjectId, x.Priority });
            entity.HasOne<Project>().WithMany().HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.Cascade);
        });
    }
}
