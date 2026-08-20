using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IngSoftStudio.Infrastructure.Persistence.Migrations;

[DbContext(typeof(IngSoftStudioDbContext))]
[Migration("20260820183000_AddProjectOwnership")]
public partial class AddProjectOwnership : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<Guid>(
            name: "OwnerId",
            table: "Projects",
            type: "uniqueidentifier",
            nullable: false,
            defaultValue: Guid.Empty);

        migrationBuilder.Sql("""
            DECLARE @OwnerId uniqueidentifier = (SELECT TOP 1 Id FROM Users ORDER BY Id);
            IF @OwnerId IS NOT NULL UPDATE Projects SET OwnerId = @OwnerId WHERE OwnerId = '00000000-0000-0000-0000-000000000000';
            """);

        migrationBuilder.CreateIndex(
            name: "IX_Projects_OwnerId",
            table: "Projects",
            column: "OwnerId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(name: "IX_Projects_OwnerId", table: "Projects");
        migrationBuilder.DropColumn(name: "OwnerId", table: "Projects");
    }
}
