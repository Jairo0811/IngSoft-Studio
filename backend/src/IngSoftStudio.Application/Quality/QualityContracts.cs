using IngSoftStudio.Domain.Quality;

namespace IngSoftStudio.Application.Quality;

public sealed record CreateRiskRequest(string Title, string Description, RiskProbability Probability, RiskImpact Impact, string Mitigation);
public sealed record RiskResponse(Guid Id, Guid ProjectId, string Title, string Description, RiskProbability Probability, RiskImpact Impact, RiskStatus Status, int Score, string Mitigation, DateTime CreatedAtUtc, DateTime? UpdatedAtUtc);
public sealed record ChangeRiskStatusRequest(RiskStatus Status);

public sealed record CreateTestCaseRequest(Guid? RequirementId, string Title, string Preconditions, string Steps, string ExpectedResult);
public sealed record ExecuteTestCaseRequest(TestCaseStatus Status, string? ActualResult);
public sealed record TestCaseResponse(Guid Id, Guid ProjectId, Guid? RequirementId, string Title, string Preconditions, string Steps, string ExpectedResult, string? ActualResult, TestCaseStatus Status, DateTime CreatedAtUtc, DateTime? ExecutedAtUtc);

public sealed record CreateDefectRequest(Guid? RequirementId, Guid? TestCaseId, string Title, string Description, DefectSeverity Severity, DefectPriority Priority);
public sealed record ChangeDefectStatusRequest(DefectStatus Status);
public sealed record DefectResponse(Guid Id, Guid ProjectId, Guid? RequirementId, Guid? TestCaseId, string Title, string Description, DefectSeverity Severity, DefectPriority Priority, DefectStatus Status, DateTime CreatedAtUtc, DateTime? UpdatedAtUtc);

public sealed record TraceabilityRow(Guid RequirementId, string RequirementTitle, string RequirementStatus, int TestCases, int PassedTests, int FailedTests, int OpenDefects, bool Covered);
public sealed record QualityMetrics(int TotalRequirements, int CoveredRequirements, decimal RequirementCoveragePercent, int TotalTests, int PassedTests, int FailedTests, decimal TestPassRatePercent, int OpenDefects, int CriticalDefects, int OpenRisks, int HighRisks);
public sealed record QualityDashboard(QualityMetrics Metrics, IReadOnlyCollection<TraceabilityRow> Traceability, IReadOnlyCollection<RiskResponse> Risks, IReadOnlyCollection<TestCaseResponse> TestCases, IReadOnlyCollection<DefectResponse> Defects);

public interface IQualityService
{
    Task<QualityDashboard> GetDashboardAsync(Guid projectId, CancellationToken cancellationToken);
    Task<RiskResponse> CreateRiskAsync(Guid projectId, CreateRiskRequest request, CancellationToken cancellationToken);
    Task<RiskResponse?> ChangeRiskStatusAsync(Guid projectId, Guid riskId, RiskStatus status, CancellationToken cancellationToken);
    Task<TestCaseResponse> CreateTestCaseAsync(Guid projectId, CreateTestCaseRequest request, CancellationToken cancellationToken);
    Task<TestCaseResponse?> ExecuteTestCaseAsync(Guid projectId, Guid testCaseId, ExecuteTestCaseRequest request, CancellationToken cancellationToken);
    Task<DefectResponse> CreateDefectAsync(Guid projectId, CreateDefectRequest request, CancellationToken cancellationToken);
    Task<DefectResponse?> ChangeDefectStatusAsync(Guid projectId, Guid defectId, DefectStatus status, CancellationToken cancellationToken);
}
