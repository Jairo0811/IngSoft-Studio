using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IngSoftStudio.Infrastructure.Persistence.Migrations;

[DbContext(typeof(IngSoftStudioDbContext))]
[Migration("20260820180000_AddProjectOwnership")]
public partial class AddProjectOwnership : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<Guid>(
            name: "OwnerId",
            table: "Projects",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.CreateIndex(
            name: "IX_Projects_OwnerId",
            table: "Projects",
            column: "OwnerId");

        migrationBuilder.AddForeignKey(
            name: "FK_Projects_Users_OwnerId",
            table: "Projects",
            column: "OwnerId",
            principalTable: "Users",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Projects_Users_OwnerId",
            table: "Projects");

        migrationBuilder.DropIndex(
            name: "IX_Projects_OwnerId",
            table: "Projects");

        migrationBuilder.DropColumn(
            name: "OwnerId",
            table: "Projects");
    }
}
