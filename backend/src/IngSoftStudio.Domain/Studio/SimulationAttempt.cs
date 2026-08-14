namespace IngSoftStudio.Domain.Studio;

public sealed class SimulationAttempt
{
    private SimulationAttempt() { }

    public SimulationAttempt(Guid userId, string scenarioId, string optionId, int score, string level)
    {
        if (userId == Guid.Empty) throw new ArgumentException("User is required.", nameof(userId));
        if (string.IsNullOrWhiteSpace(scenarioId)) throw new ArgumentException("Scenario is required.", nameof(scenarioId));
        if (string.IsNullOrWhiteSpace(optionId)) throw new ArgumentException("Option is required.", nameof(optionId));
        if (score is < 0 or > 100) throw new ArgumentOutOfRangeException(nameof(score));

        Id = Guid.NewGuid();
        UserId = userId;
        ScenarioId = scenarioId.Trim();
        OptionId = optionId.Trim();
        Score = score;
        Level = level.Trim();
        CreatedAtUtc = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string ScenarioId { get; private set; } = string.Empty;
    public string OptionId { get; private set; } = string.Empty;
    public int Score { get; private set; }
    public string Level { get; private set; } = string.Empty;
    public DateTime CreatedAtUtc { get; private set; }
}
