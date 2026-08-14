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

public sealed record SimulationOption(string Id, string Label, int Score, string Feedback);
public sealed record SimulationScenario(string Id, string Title, string Context, string Question, IReadOnlyCollection<SimulationOption> Options);
public sealed record EvaluateSimulationRequest(string ScenarioId, string OptionId);
public sealed record SimulationResult(string ScenarioId, string OptionId, int Score, string Feedback, string Level);

public sealed record LearningTopic(string Id, string Title, string Category, string Summary, IReadOnlyCollection<string> KeyPoints);

public interface IStudioService
{
    Task<PortfolioDashboard> GetDashboardAsync(CancellationToken cancellationToken);
    IReadOnlyCollection<SimulationScenario> GetScenarios();
    SimulationResult? Evaluate(EvaluateSimulationRequest request);
    IReadOnlyCollection<LearningTopic> GetLearningTopics();
}
