using IngSoftStudio.Domain.Projects;
using IngSoftStudio.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IngSoftStudio.Infrastructure.Persistence;

public sealed class IngSoftStudioDbContext(DbContextOptions<IngSoftStudioDbContext> options)
    : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>(options)
{
    public DbSet<Project> Projects => Set<Project>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ApplicationUser>(builder =>
        {
            builder.ToTable("Users");
            builder.Property(user => user.FullName).HasMaxLength(150).IsRequired();
        });

        modelBuilder.Entity<IdentityRole<Guid>>().ToTable("Roles");
        modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("UserRoles");
        modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims");
        modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogins");
        modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaims");
        modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("UserTokens");

        modelBuilder.Entity<Project>(builder =>
        {
            builder.ToTable("Projects");
            builder.HasKey(project => project.Id);
            builder.Property(project => project.Name).HasMaxLength(150).IsRequired();
            builder.Property(project => project.Description).HasMaxLength(1000);
            builder.Property(project => project.Status).HasConversion<string>().HasMaxLength(30);
            builder.HasIndex(project => project.Name);
        });
    }
}
