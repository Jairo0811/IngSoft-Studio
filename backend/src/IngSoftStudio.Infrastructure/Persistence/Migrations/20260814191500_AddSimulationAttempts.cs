using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IngSoftStudio.Infrastructure.Persistence.Migrations;

[DbContext(typeof(IngSoftStudioDbContext))]
[Migration("20260814191500_AddSimulationAttempts")]
#pragma warning disable CA1861
public partial class AddSimulationAttempts : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "SimulationAttempts",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                ScenarioId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                OptionId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                Score = table.Column<int>(type: "int", nullable: false),
                Level = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_SimulationAttempts", x => x.Id);
                table.ForeignKey("FK_SimulationAttempts_Users_UserId", x => x.UserId, "Users", "Id", onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_SimulationAttempts_UserId_CreatedAtUtc",
            table: "SimulationAttempts",
            columns: new[] { "UserId", "CreatedAtUtc" });
    }

    protected override void Down(MigrationBuilder migrationBuilder) => migrationBuilder.DropTable(name: "SimulationAttempts");
}
#pragma warning restore CA1861
