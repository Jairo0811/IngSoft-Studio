using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IngSoftStudio.Infrastructure.Persistence.Migrations;

public partial class AddQualityManagement : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Risks",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                Probability = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                Impact = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                Mitigation = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Risks", x => x.Id);
                table.ForeignKey("FK_Risks_Projects_ProjectId", x => x.ProjectId, "Projects", "Id", onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "TestCases",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                RequirementId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                Preconditions = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                Steps = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                ExpectedResult = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                ActualResult = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                ExecutedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_TestCases", x => x.Id);
                table.ForeignKey("FK_TestCases_Projects_ProjectId", x => x.ProjectId, "Projects", "Id", onDelete: ReferentialAction.Cascade);
                table.ForeignKey("FK_TestCases_Requirements_RequirementId", x => x.RequirementId, "Requirements", "Id", onDelete: ReferentialAction.NoAction);
            });

        migrationBuilder.CreateTable(
            name: "Defects",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                RequirementId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                TestCaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                Severity = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                Priority = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Defects", x => x.Id);
                table.ForeignKey("FK_Defects_Projects_ProjectId", x => x.ProjectId, "Projects", "Id", onDelete: ReferentialAction.Cascade);
                table.ForeignKey("FK_Defects_Requirements_RequirementId", x => x.RequirementId, "Requirements", "Id", onDelete: ReferentialAction.NoAction);
                table.ForeignKey("FK_Defects_TestCases_TestCaseId", x => x.TestCaseId, "TestCases", "Id", onDelete: ReferentialAction.NoAction);
            });

        migrationBuilder.CreateIndex("IX_Risks_ProjectId_Status", "Risks", new[] { "ProjectId", "Status" });
        migrationBuilder.CreateIndex("IX_TestCases_ProjectId_RequirementId", "TestCases", new[] { "ProjectId", "RequirementId" });
        migrationBuilder.CreateIndex("IX_TestCases_RequirementId", "TestCases", "RequirementId");
        migrationBuilder.CreateIndex("IX_Defects_ProjectId_Status_Severity", "Defects", new[] { "ProjectId", "Status", "Severity" });
        migrationBuilder.CreateIndex("IX_Defects_RequirementId", "Defects", "RequirementId");
        migrationBuilder.CreateIndex("IX_Defects_TestCaseId", "Defects", "TestCaseId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "Defects");
        migrationBuilder.DropTable(name: "Risks");
        migrationBuilder.DropTable(name: "TestCases");
    }
}
