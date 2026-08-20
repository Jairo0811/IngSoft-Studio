using System.ComponentModel.DataAnnotations;
using IngSoftStudio.Domain.Quality;

namespace IngSoftStudio.Application.Quality;

public sealed record CreateRiskRequest(
    [Required, StringLength(200, MinimumLength = 2)] string Title,
    [Required, StringLength(2000, MinimumLength = 2)] string Description,
    [EnumDataType(typeof(RiskProbability))] RiskProbability Probability,
    [EnumDataType(typeof(RiskImpact))] RiskImpact Impact,
    [Required, StringLength(2000, MinimumLength = 2)] string Mitigation);
public sealed record RiskResponse(Guid Id, Guid ProjectId, string Title, string Description, RiskProbability Probability, RiskImpact Impact, RiskStatus Status, int Score, string Mitigation, DateTime CreatedAtUtc, DateTime? UpdatedAtUtc);
public sealed record ChangeRiskStatusRequest(
    [EnumDataType(typeof(RiskStatus))] RiskStatus Status);

public sealed record CreateTestCaseRequest(
    Guid? RequirementId,
    [Required, StringLength(200, MinimumLength = 2)] string Title,
    [Required, StringLength(2000)] string Preconditions,
    [Required, StringLength(4000, MinimumLength = 2)] string Steps,
    [Required, StringLength(2000, MinimumLength = 2)] string ExpectedResult);

public sealed record ExecuteTestCaseRequest(
    [EnumDataType(typeof(TestCaseStatus))] TestCaseStatus Status,
    [StringLength(2000)] string? ActualResult);
public sealed record TestCaseResponse(Guid Id, Guid ProjectId, Guid? RequirementId, string Title, string Preconditions, string Steps, string ExpectedResult, string? ActualResult, TestCaseStatus Status, DateTime CreatedAtUtc, DateTime? ExecutedAtUtc);

public sealed record CreateDefectRequest(
    Guid? RequirementId,
    Guid? TestCaseId,
    [Required, StringLength(200, MinimumLength = 2)] string Title,
    [Required, StringLength(4000, MinimumLength = 2)] string Description,
    [EnumDataType(typeof(DefectSeverity))] DefectSeverity Severity,
    [EnumDataType(typeof(DefectPriority))] DefectPriority Priority);

public sealed record ChangeDefectStatusRequest(
    [EnumDataType(typeof(DefectStatus))] DefectStatus Status);
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
