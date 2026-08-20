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
            .HasAnnotation("ProductVersion", "10.0.9")
            .HasAnnotation("Relational:MaxIdentifierLength", 128);

        SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

        modelBuilder.Entity("IngSoftStudio.Domain.Projects.Project", b =>
        {
            b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
            b.Property<DateTime>("CreatedAtUtc").HasColumnType("datetime2");
            b.Property<string>("Description").HasMaxLength(1000).HasColumnType("nvarchar(1000)");
            b.Property<string>("Name").IsRequired().HasMaxLength(150).HasColumnType("nvarchar(150)");
            b.Property<Guid?>("OwnerId").HasColumnType("uniqueidentifier");
            b.Property<string>("Status").IsRequired().HasMaxLength(30).HasColumnType("nvarchar(30)");
            b.Property<DateTime?>("UpdatedAtUtc").HasColumnType("datetime2");
            b.HasKey("Id");
            b.HasIndex("Name");
            b.HasIndex("OwnerId");
            b.ToTable("Projects");
        });

        modelBuilder.Entity("IngSoftStudio.Domain.Projects.Project", b =>
        {
            b.HasOne("IngSoftStudio.Infrastructure.Identity.ApplicationUser", null)
                .WithMany()
                .HasForeignKey("OwnerId")
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity("IngSoftStudio.Domain.Requirements.Requirement", b =>
        {
            b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
            b.Property<string>("AcceptanceCriteria").HasMaxLength(4000).HasColumnType("nvarchar(4000)");
            b.Property<DateTime>("CreatedAtUtc").HasColumnType("datetime2");
            b.Property<string>("Description").IsRequired().HasMaxLength(4000).HasColumnType("nvarchar(4000)");
            b.Property<string>("Priority").IsRequired().HasMaxLength(20).HasColumnType("nvarchar(20)");
            b.Property<Guid>("ProjectId").HasColumnType("uniqueidentifier");
            b.Property<string>("Source").HasMaxLength(500).HasColumnType("nvarchar(500)");
            b.Property<string>("Status").IsRequired().HasMaxLength(30).HasColumnType("nvarchar(30)");
            b.Property<string>("Title").IsRequired().HasMaxLength(200).HasColumnType("nvarchar(200)");
            b.Property<string>("Type").IsRequired().HasMaxLength(30).HasColumnType("nvarchar(30)");
            b.Property<DateTime?>("UpdatedAtUtc").HasColumnType("datetime2");
            b.HasKey("Id");
            b.HasIndex("ProjectId", "Priority");
            b.ToTable("Requirements");
        });

        modelBuilder.Entity("IngSoftStudio.Domain.Quality.Risk", b =>
        {
            b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
            b.Property<DateTime>("CreatedAtUtc").HasColumnType("datetime2");
            b.Property<string>("Description").IsRequired().HasMaxLength(2000).HasColumnType("nvarchar(2000)");
            b.Property<string>("Impact").IsRequired().HasMaxLength(20).HasColumnType("nvarchar(20)");
            b.Property<string>("Mitigation").IsRequired().HasMaxLength(2000).HasColumnType("nvarchar(2000)");
            b.Property<string>("Probability").IsRequired().HasMaxLength(20).HasColumnType("nvarchar(20)");
            b.Property<Guid>("ProjectId").HasColumnType("uniqueidentifier");
            b.Property<string>("Status").IsRequired().HasMaxLength(20).HasColumnType("nvarchar(20)");
            b.Property<string>("Title").IsRequired().HasMaxLength(200).HasColumnType("nvarchar(200)");
            b.Property<DateTime?>("UpdatedAtUtc").HasColumnType("datetime2");
            b.HasKey("Id");
            b.HasIndex("ProjectId", "Status");
            b.ToTable("Risks");
        });

        modelBuilder.Entity("IngSoftStudio.Domain.Quality.TestCase", b =>
        {
            b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
            b.Property<string>("ActualResult").HasMaxLength(2000).HasColumnType("nvarchar(2000)");
            b.Property<DateTime>("CreatedAtUtc").HasColumnType("datetime2");
            b.Property<DateTime?>("ExecutedAtUtc").HasColumnType("datetime2");
            b.Property<string>("ExpectedResult").IsRequired().HasMaxLength(2000).HasColumnType("nvarchar(2000)");
            b.Property<string>("Preconditions").IsRequired().HasMaxLength(2000).HasColumnType("nvarchar(2000)");
            b.Property<Guid>("ProjectId").HasColumnType("uniqueidentifier");
            b.Property<Guid?>("RequirementId").HasColumnType("uniqueidentifier");
            b.Property<string>("Status").IsRequired().HasMaxLength(20).HasColumnType("nvarchar(20)");
            b.Property<string>("Steps").IsRequired().HasMaxLength(4000).HasColumnType("nvarchar(4000)");
            b.Property<string>("Title").IsRequired().HasMaxLength(200).HasColumnType("nvarchar(200)");
            b.HasKey("Id");
            b.HasIndex("RequirementId");
            b.HasIndex("ProjectId", "RequirementId");
            b.ToTable("TestCases");
        });

        modelBuilder.Entity("IngSoftStudio.Domain.Quality.Defect", b =>
        {
            b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
            b.Property<DateTime>("CreatedAtUtc").HasColumnType("datetime2");
            b.Property<string>("Description").IsRequired().HasMaxLength(4000).HasColumnType("nvarchar(4000)");
            b.Property<string>("Priority").IsRequired().HasMaxLength(20).HasColumnType("nvarchar(20)");
            b.Property<Guid>("ProjectId").HasColumnType("uniqueidentifier");
            b.Property<Guid?>("RequirementId").HasColumnType("uniqueidentifier");
            b.Property<string>("Severity").IsRequired().HasMaxLength(20).HasColumnType("nvarchar(20)");
            b.Property<string>("Status").IsRequired().HasMaxLength(20).HasColumnType("nvarchar(20)");
            b.Property<Guid?>("TestCaseId").HasColumnType("uniqueidentifier");
            b.Property<string>("Title").IsRequired().HasMaxLength(200).HasColumnType("nvarchar(200)");
            b.Property<DateTime?>("UpdatedAtUtc").HasColumnType("datetime2");
            b.HasKey("Id");
            b.HasIndex("RequirementId");
            b.HasIndex("TestCaseId");
            b.HasIndex("ProjectId", "Status", "Severity");
            b.ToTable("Defects");
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
            b.HasKey("Id");
            b.HasIndex("UserId", "CreatedAtUtc");
            b.ToTable("SimulationAttempts");
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
            b.Property<string>("PhoneNumber").HasMaxLength(256).HasColumnType("nvarchar(256)");
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
            b.HasKey("Id");
            b.HasIndex("NormalizedName").IsUnique().HasDatabaseName("RoleNameIndex").HasFilter("[NormalizedName] IS NOT NULL");
            b.ToTable("Roles");
        });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
        {
            b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int");
            SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));
            b.Property<string>("ClaimType").HasColumnType("nvarchar(max)");
            b.Property<string>("ClaimValue").HasColumnType("nvarchar(max)");
            b.Property<Guid>("RoleId").HasColumnType("uniqueidentifier");
            b.HasKey("Id");
            b.HasIndex("RoleId");
            b.ToTable("RoleClaims");
        });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
        {
            b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int");
            SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));
            b.Property<string>("ClaimType").HasColumnType("nvarchar(max)");
            b.Property<string>("ClaimValue").HasColumnType("nvarchar(max)");
            b.Property<Guid>("UserId").HasColumnType("uniqueidentifier");
            b.HasKey("Id");
            b.HasIndex("UserId");
            b.ToTable("UserClaims");
        });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
        {
            b.Property<string>("LoginProvider").HasMaxLength(128).HasColumnType("nvarchar(128)");
            b.Property<string>("ProviderKey").HasMaxLength(128).HasColumnType("nvarchar(128)");
            b.Property<string>("ProviderDisplayName").HasColumnType("nvarchar(max)");
            b.Property<Guid>("UserId").HasColumnType("uniqueidentifier");
            b.HasKey("LoginProvider", "ProviderKey");
            b.HasIndex("UserId");
            b.ToTable("UserLogins");
        });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
        {
            b.Property<Guid>("UserId").HasColumnType("uniqueidentifier");
            b.Property<Guid>("RoleId").HasColumnType("uniqueidentifier");
            b.HasKey("UserId", "RoleId");
            b.HasIndex("RoleId");
            b.ToTable("UserRoles");
        });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
        {
            b.Property<Guid>("UserId").HasColumnType("uniqueidentifier");
            b.Property<string>("LoginProvider").HasMaxLength(128).HasColumnType("nvarchar(128)");
            b.Property<string>("Name").HasMaxLength(128).HasColumnType("nvarchar(128)");
            b.Property<string>("Value").HasColumnType("nvarchar(max)");
            b.HasKey("UserId", "LoginProvider", "Name");
            b.ToTable("UserTokens");
        });

        modelBuilder.Entity("IngSoftStudio.Domain.Requirements.Requirement", b =>
        {
            b.HasOne("IngSoftStudio.Domain.Projects.Project", null)
                .WithMany()
                .HasForeignKey("ProjectId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        });

        modelBuilder.Entity("IngSoftStudio.Domain.Quality.Risk", b =>
        {
            b.HasOne("IngSoftStudio.Domain.Projects.Project", null)
                .WithMany()
                .HasForeignKey("ProjectId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        });

        modelBuilder.Entity("IngSoftStudio.Domain.Quality.TestCase", b =>
        {
            b.HasOne("IngSoftStudio.Domain.Projects.Project", null)
                .WithMany()
                .HasForeignKey("ProjectId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            b.HasOne("IngSoftStudio.Domain.Requirements.Requirement", null)
                .WithMany()
                .HasForeignKey("RequirementId")
                .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity("IngSoftStudio.Domain.Quality.Defect", b =>
        {
            b.HasOne("IngSoftStudio.Domain.Projects.Project", null)
                .WithMany()
                .HasForeignKey("ProjectId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            b.HasOne("IngSoftStudio.Domain.Requirements.Requirement", null)
                .WithMany()
                .HasForeignKey("RequirementId")
                .OnDelete(DeleteBehavior.NoAction);

            b.HasOne("IngSoftStudio.Domain.Quality.TestCase", null)
                .WithMany()
                .HasForeignKey("TestCaseId")
                .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity("IngSoftStudio.Domain.Studio.SimulationAttempt", b =>
        {
            b.HasOne("IngSoftStudio.Infrastructure.Identity.ApplicationUser", null)
                .WithMany()
                .HasForeignKey("UserId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
        {
            b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", null).WithMany().HasForeignKey("RoleId").OnDelete(DeleteBehavior.Cascade).IsRequired();
        });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
        {
            b.HasOne("IngSoftStudio.Infrastructure.Identity.ApplicationUser", null).WithMany().HasForeignKey("UserId").OnDelete(DeleteBehavior.Cascade).IsRequired();
        });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
        {
            b.HasOne("IngSoftStudio.Infrastructure.Identity.ApplicationUser", null).WithMany().HasForeignKey("UserId").OnDelete(DeleteBehavior.Cascade).IsRequired();
        });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
        {
            b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", null).WithMany().HasForeignKey("RoleId").OnDelete(DeleteBehavior.Cascade).IsRequired();
            b.HasOne("IngSoftStudio.Infrastructure.Identity.ApplicationUser", null).WithMany().HasForeignKey("UserId").OnDelete(DeleteBehavior.Cascade).IsRequired();
        });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
        {
            b.HasOne("IngSoftStudio.Infrastructure.Identity.ApplicationUser", null).WithMany().HasForeignKey("UserId").OnDelete(DeleteBehavior.Cascade).IsRequired();
        });
#pragma warning restore 612, 618
    }
}
