using IngSoftStudio.Application.Quality;
using IngSoftStudio.Domain.Quality;
using IngSoftStudio.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace IngSoftStudio.Infrastructure.Quality;

public sealed class QualityService(IngSoftStudioDbContext dbContext) : IQualityService
{
    public async Task<QualityDashboard> GetDashboardAsync(Guid projectId, CancellationToken cancellationToken)
    {
        await EnsureProjectExistsAsync(projectId, cancellationToken);

        var requirements = await dbContext.Requirements.AsNoTracking().Where(x => x.ProjectId == projectId).OrderBy(x => x.Title).ToListAsync(cancellationToken);
        var tests = await dbContext.TestCases.AsNoTracking().Where(x => x.ProjectId == projectId).OrderByDescending(x => x.CreatedAtUtc).ToListAsync(cancellationToken);
        var defects = await dbContext.Defects.AsNoTracking().Where(x => x.ProjectId == projectId).OrderByDescending(x => x.CreatedAtUtc).ToListAsync(cancellationToken);
        var risks = await dbContext.Risks.AsNoTracking().Where(x => x.ProjectId == projectId).OrderByDescending(x => x.CreatedAtUtc).ToListAsync(cancellationToken);

        var traceability = requirements.Select(requirement =>
        {
            var requirementTests = tests.Where(x => x.RequirementId == requirement.Id).ToArray();
            var requirementDefects = defects.Where(x => x.RequirementId == requirement.Id || requirementTests.Any(t => t.Id == x.TestCaseId)).ToArray();
            return new TraceabilityRow(
                requirement.Id,
                requirement.Title,
                requirement.Status.ToString(),
                requirementTests.Length,
                requirementTests.Count(x => x.Status == TestCaseStatus.Passed),
                requirementTests.Count(x => x.Status == TestCaseStatus.Failed),
                requirementDefects.Count(x => x.Status is DefectStatus.Open or DefectStatus.InProgress),
                requirementTests.Length > 0);
        }).ToArray();

        var covered = traceability.Count(x => x.Covered);
        var totalTests = tests.Count;
        var passedTests = tests.Count(x => x.Status == TestCaseStatus.Passed);
        var failedTests = tests.Count(x => x.Status == TestCaseStatus.Failed);
        var metrics = new QualityMetrics(
            requirements.Count,
            covered,
            Percentage(covered, requirements.Count),
            totalTests,
            passedTests,
            failedTests,
            Percentage(passedTests, tests.Count(x => x.Status != TestCaseStatus.NotRun)),
            defects.Count(x => x.Status is DefectStatus.Open or DefectStatus.InProgress),
            defects.Count(x => x.Severity == DefectSeverity.Critical && x.Status is not DefectStatus.Closed),
            risks.Count(x => x.Status is RiskStatus.Open or RiskStatus.Mitigating),
            risks.Count(x => x.Score >= 6 && x.Status is not RiskStatus.Closed));

        return new QualityDashboard(metrics, traceability, risks.Select(MapRisk).ToArray(), tests.Select(MapTestCase).ToArray(), defects.Select(MapDefect).ToArray());
    }

    public async Task<RiskResponse> CreateRiskAsync(Guid projectId, CreateRiskRequest request, CancellationToken cancellationToken)
    {
        await EnsureProjectExistsAsync(projectId, cancellationToken);
        var risk = new Risk(projectId, request.Title, request.Description, request.Probability, request.Impact, request.Mitigation);
        dbContext.Risks.Add(risk);
        await dbContext.SaveChangesAsync(cancellationToken);
        return MapRisk(risk);
    }

    public async Task<RiskResponse?> ChangeRiskStatusAsync(Guid projectId, Guid riskId, RiskStatus status, CancellationToken cancellationToken)
    {
        var risk = await dbContext.Risks.SingleOrDefaultAsync(x => x.ProjectId == projectId && x.Id == riskId, cancellationToken);
        if (risk is null) return null;
        risk.ChangeStatus(status);
        await dbContext.SaveChangesAsync(cancellationToken);
        return MapRisk(risk);
    }

    public async Task<TestCaseResponse> CreateTestCaseAsync(Guid projectId, CreateTestCaseRequest request, CancellationToken cancellationToken)
    {
        await EnsureProjectExistsAsync(projectId, cancellationToken);
        if (request.RequirementId.HasValue && !await dbContext.Requirements.AnyAsync(x => x.ProjectId == projectId && x.Id == request.RequirementId.Value, cancellationToken)) throw new KeyNotFoundException("Requirement not found.");
        var testCase = new TestCase(projectId, request.RequirementId, request.Title, request.Preconditions, request.Steps, request.ExpectedResult);
        dbContext.TestCases.Add(testCase);
        await dbContext.SaveChangesAsync(cancellationToken);
        return MapTestCase(testCase);
    }

    public async Task<TestCaseResponse?> ExecuteTestCaseAsync(Guid projectId, Guid testCaseId, ExecuteTestCaseRequest request, CancellationToken cancellationToken)
    {
        var testCase = await dbContext.TestCases.SingleOrDefaultAsync(x => x.ProjectId == projectId && x.Id == testCaseId, cancellationToken);
        if (testCase is null) return null;
        testCase.Execute(request.Status, request.ActualResult);
        await dbContext.SaveChangesAsync(cancellationToken);
        return MapTestCase(testCase);
    }

    public async Task<DefectResponse> CreateDefectAsync(Guid projectId, CreateDefectRequest request, CancellationToken cancellationToken)
    {
        await EnsureProjectExistsAsync(projectId, cancellationToken);
        if (request.RequirementId.HasValue && !await dbContext.Requirements.AnyAsync(x => x.ProjectId == projectId && x.Id == request.RequirementId.Value, cancellationToken)) throw new KeyNotFoundException("Requirement not found.");
        if (request.TestCaseId.HasValue && !await dbContext.TestCases.AnyAsync(x => x.ProjectId == projectId && x.Id == request.TestCaseId.Value, cancellationToken)) throw new KeyNotFoundException("Test case not found.");
        var defect = new Defect(projectId, request.RequirementId, request.TestCaseId, request.Title, request.Description, request.Severity, request.Priority);
        dbContext.Defects.Add(defect);
        await dbContext.SaveChangesAsync(cancellationToken);
        return MapDefect(defect);
    }

    public async Task<DefectResponse?> ChangeDefectStatusAsync(Guid projectId, Guid defectId, DefectStatus status, CancellationToken cancellationToken)
    {
        var defect = await dbContext.Defects.SingleOrDefaultAsync(x => x.ProjectId == projectId && x.Id == defectId, cancellationToken);
        if (defect is null) return null;
        defect.ChangeStatus(status);
        await dbContext.SaveChangesAsync(cancellationToken);
        return MapDefect(defect);
    }

    private async Task EnsureProjectExistsAsync(Guid projectId, CancellationToken cancellationToken)
    {
        if (!await dbContext.Projects.AnyAsync(x => x.Id == projectId, cancellationToken)) throw new KeyNotFoundException("Project not found.");
    }

    private static decimal Percentage(int value, int total) => total == 0 ? 0 : Math.Round(value * 100m / total, 2);
    private static RiskResponse MapRisk(Risk x) => new(x.Id, x.ProjectId, x.Title, x.Description, x.Probability, x.Impact, x.Status, x.Score, x.Mitigation, x.CreatedAtUtc, x.UpdatedAtUtc);
    private static TestCaseResponse MapTestCase(TestCase x) => new(x.Id, x.ProjectId, x.RequirementId, x.Title, x.Preconditions, x.Steps, x.ExpectedResult, x.ActualResult, x.Status, x.CreatedAtUtc, x.ExecutedAtUtc);
    private static DefectResponse MapDefect(Defect x) => new(x.Id, x.ProjectId, x.RequirementId, x.TestCaseId, x.Title, x.Description, x.Severity, x.Priority, x.Status, x.CreatedAtUtc, x.UpdatedAtUtc);
}
