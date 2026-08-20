using System.ComponentModel.DataAnnotations;

namespace IngSoftStudio.Application.Studio;

public sealed record PortfolioDashboard(
    int TotalProjects,
    int DraftProjects,
    int ActiveProjects,
    int CompletedProjects,
    int ArchivedProjects,
    int TotalRequirements,
    int TotalTests,
    int PassedTests,
    int OpenDefects,
    int OpenRisks,
    decimal TestPassRatePercent,
    decimal RequirementCoveragePercent);

public sealed record ProjectInsight(Guid ProjectId, string ProjectName, string Status, int Requirements, int Tests, int PassedTests, int OpenDefects, int OpenRisks, decimal CoveragePercent, decimal PassRatePercent);
public sealed record PortfolioTrend(string Label, int Requirements, int Tests, int Defects, int Risks);

public sealed record SimulationOption(string Id, string Label, int Score, string Feedback);
public sealed record SimulationScenario(string Id, string Title, string Context, string Question, IReadOnlyCollection<SimulationOption> Options);
public sealed record EvaluateSimulationRequest(
    [property: Required, StringLength(100)] string ScenarioId,
    [property: Required, StringLength(100)] string OptionId);
public sealed record SimulationResult(string ScenarioId, string OptionId, int Score, string Feedback, string Level);
public sealed record SimulationAttemptResponse(Guid Id, string ScenarioId, string OptionId, int Score, string Level, DateTime CreatedAtUtc);
public sealed record SimulationSummary(int Attempts, decimal AverageScore, int BestScore, IReadOnlyCollection<SimulationAttemptResponse> RecentAttempts);

public sealed record LearningTopic(string Id, string Title, string Category, string Summary, IReadOnlyCollection<string> KeyPoints);
public sealed record ReportFile(byte[] Content, string ContentType, string FileName);

public interface IStudioService
{
    Task<PortfolioDashboard> GetDashboardAsync(CancellationToken cancellationToken);
    Task<IReadOnlyCollection<ProjectInsight>> GetProjectInsightsAsync(CancellationToken cancellationToken);
    Task<IReadOnlyCollection<PortfolioTrend>> GetTrendsAsync(CancellationToken cancellationToken);
    IReadOnlyCollection<SimulationScenario> GetScenarios();
    Task<SimulationResult?> EvaluateAsync(Guid userId, EvaluateSimulationRequest request, CancellationToken cancellationToken);
    Task<SimulationSummary> GetSimulationSummaryAsync(Guid userId, CancellationToken cancellationToken);
    IReadOnlyCollection<LearningTopic> GetLearningTopics();
    Task<ReportFile> BuildPdfReportAsync(CancellationToken cancellationToken);
    Task<ReportFile> BuildExcelReportAsync(CancellationToken cancellationToken);
}
