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
            b.HasKey("Id");
            b.HasIndex("Name");
            b.ToTable("Projects");
        });
#pragma warning restore 612, 618
    }
}
