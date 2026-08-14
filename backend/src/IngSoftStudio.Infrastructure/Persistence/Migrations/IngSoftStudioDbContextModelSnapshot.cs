using IngSoftStudio.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace IngSoftStudio.Infrastructure.Persistence.Migrations;

[DbContext(typeof(IngSoftStudioDbContext))]
internal sealed class IngSoftStudioDbContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
#pragma warning disable 612, 618
        modelBuilder
            .HasAnnotation("ProductVersion", "10.0.8")
            .HasAnnotation("Relational:MaxIdentifierLength", 128);

        SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

        modelBuilder.Entity("IngSoftStudio.Domain.Projects.Project", b =>
        {
            b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
            b.Property<DateTime>("CreatedAtUtc").HasColumnType("datetime2");
            b.Property<string>("Description").HasMaxLength(1000).HasColumnType("nvarchar(1000)");
            b.Property<string>("Name").IsRequired().HasMaxLength(150).HasColumnType("nvarchar(150)");
            b.Property<string>("Status").IsRequired().HasMaxLength(30).HasColumnType("nvarchar(30)");
            b.Property<DateTime?>("UpdatedAtUtc").HasColumnType("datetime2");
            b.HasKey("Id"); b.HasIndex("Name"); b.ToTable("Projects");
        });

        modelBuilder.Entity("IngSoftStudio.Domain.Requirements.Requirement", b =>
        {
            b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
            b.Property<string>("AcceptanceCriteria").HasMaxLength(4000).HasColumnType("nvarchar(4000)");
            b.Property<DateTime>("CreatedAtUtc").HasColumnType("datetime2");
            b.Property<string>("Description").IsRequired().HasMaxLength(4000).HasColumnType("nvarchar(4000)");
            b.Property<Guid>("ProjectId").HasColumnType("uniqueidentifier");
            b.Property<string>("Source").HasMaxLength(500).HasColumnType("nvarchar(500)");
            b.Property<DateTime?>("UpdatedAtUtc").HasColumnType("datetime2");
            b.Property<string>("Title").IsRequired().HasMaxLength(200).HasColumnType("nvarchar(200)");
            b.HasKey("Id"); b.HasIndex("ProjectId", "Priority"); b.ToTable("Requirements");
        });

        modelBuilder.Entity("IngSoftStudio.Domain.Studio.SimulationAttempt", b =>
        {
            b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
            b.Property<DateTime>("CreatedAtUtc").HasColumnType("datetime2");
            b.Property<string>("Level").IsRequired().HasMaxLength(40).HasColumnType("nvarchar(40)");
            b.Property<string>("OptionId").IsRequired().HasMaxLength(100).HasColumnType("nvarchar(100)");
            b.Property<string>("ScenarioId").IsRequired().HasMaxLength(100).HasColumnType("nvarchar(100)");
            b.Property<int>("Score").HasColumnType("int");
            b.Property<Guid>("UserId").HasColumnType("uniqueidentifier");
            b.HasKey("Id"); b.HasIndex("UserId", "CreatedAtUtc"); b.ToTable("SimulationAttempts");
        });

        modelBuilder.Entity("IngSoftStudio.Infrastructure.Identity.ApplicationUser", b =>
        {
            b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
            b.Property<int>("AccessFailedCount").HasColumnType("int");
            b.Property<string>("ConcurrencyStamp").IsConcurrencyToken().HasColumnType("nvarchar(max)");
            b.Property<DateTime>("CreatedAtUtc").HasColumnType("datetime2");
            b.Property<string>("Email").HasMaxLength(256).HasColumnType("nvarchar(256)");
            b.Property<bool>("EmailConfirmed").HasColumnType("bit");
            b.Property<string>("FullName").IsRequired().HasMaxLength(150).HasColumnType("nvarchar(150)");
            b.Property<bool>("LockoutEnabled").HasColumnType("bit");
            b.Property<DateTimeOffset?>("LockoutEnd").HasColumnType("datetimeoffset");
            b.Property<string>("NormalizedEmail").HasMaxLength(256).HasColumnType("nvarchar(256)");
            b.Property<string>("NormalizedUserName").HasMaxLength(256).HasColumnType("nvarchar(256)");
            b.Property<string>("PasswordHash").HasColumnType("nvarchar(max)");
            b.Property<string>("PhoneNumber").HasColumnType("nvarchar(max)");
            b.Property<bool>("PhoneNumberConfirmed").HasColumnType("bit");
            b.Property<string>("SecurityStamp").HasColumnType("nvarchar(max)");
            b.Property<bool>("TwoFactorEnabled").HasColumnType("bit");
            b.Property<string>("UserName").HasMaxLength(256).HasColumnType("nvarchar(256)");
            b.HasKey("Id");
            b.HasIndex("NormalizedEmail").HasDatabaseName("EmailIndex");
            b.HasIndex("NormalizedUserName").IsUnique().HasDatabaseName("UserNameIndex").HasFilter("[NormalizedUserName] IS NOT NULL");
            b.ToTable("Users");
        });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", b =>
        {
            b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
            b.Property<string>("ConcurrencyStamp").IsConcurrencyToken().HasColumnType("nvarchar(max)");
            b.Property<string>("Name").HasMaxLength(256).HasColumnType("nvarchar(256)");
            b.Property<string>("NormalizedName").HasMaxLength(256).HasColumnType("nvarchar(256)");
            b.HasKey("Id"); b.HasIndex("NormalizedName").IsUnique().HasDatabaseName("RoleNameIndex").HasFilter("[NormalizedName] IS NOT NULL"); b.ToTable("Roles");
        });

        modelBuilder.Entity("IngSoftStudio.Domain.Studio.SimulationAttempt", b =>
        {
            b.HasOne("IngSoftStudio.Infrastructure.Identity.ApplicationUser", null).WithMany().HasForeignKey("UserId").OnDelete(DeleteBehavior.Cascade).IsRequired();
        });
#pragma warning restore 612, 618
    }
}
