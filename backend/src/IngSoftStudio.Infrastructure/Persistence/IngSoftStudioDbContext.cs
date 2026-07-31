using IngSoftStudio.Domain.Projects;
using Microsoft.EntityFrameworkCore;

namespace IngSoftStudio.Infrastructure.Persistence;

public sealed class IngSoftStudioDbContext(DbContextOptions<IngSoftStudioDbContext> options)
    : DbContext(options)
{
    public DbSet<Project> Projects => Set<Project>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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
