namespace IngSoftStudio.Domain.Quality;

public sealed class Risk
{
    private Risk() { }
    public Risk(Guid projectId, string title, string description, RiskProbability probability, RiskImpact impact, string mitigation)
    {
        if (projectId == Guid.Empty) throw new ArgumentException("Project is required.", nameof(projectId));
        Id = Guid.NewGuid(); ProjectId = projectId; CreatedAtUtc = DateTime.UtcNow;
        Update(title, description, probability, impact, mitigation);
    }
    public Guid Id { get; private set; }
    public Guid ProjectId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public RiskProbability Probability { get; private set; }
    public RiskImpact Impact { get; private set; }
    public RiskStatus Status { get; private set; } = RiskStatus.Open;
    public string Mitigation { get; private set; } = string.Empty;
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? UpdatedAtUtc { get; private set; }
    public int Score => (int)Probability * (int)Impact;
    public void Update(string title, string description, RiskProbability probability, RiskImpact impact, string mitigation)
    {
        if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Risk title is required.", nameof(title));
        Title = title.Trim(); Description = description?.Trim() ?? string.Empty; Probability = probability; Impact = impact; Mitigation = mitigation?.Trim() ?? string.Empty; UpdatedAtUtc = DateTime.UtcNow;
    }
    public void ChangeStatus(RiskStatus status) { Status = status; UpdatedAtUtc = DateTime.UtcNow; }
}

public sealed class TestCase
{
    private TestCase() { }
    public TestCase(Guid projectId, Guid? requirementId, string title, string preconditions, string steps, string expectedResult)
    {
        if (projectId == Guid.Empty) throw new ArgumentException("Project is required.", nameof(projectId));
        if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Test title is required.", nameof(title));
        Id = Guid.NewGuid(); ProjectId = projectId; RequirementId = requirementId; Title = title.Trim(); Preconditions = preconditions?.Trim() ?? string.Empty; Steps = steps?.Trim() ?? string.Empty; ExpectedResult = expectedResult?.Trim() ?? string.Empty; Status = TestCaseStatus.NotRun; CreatedAtUtc = DateTime.UtcNow;
    }
    public Guid Id { get; private set; }
    public Guid ProjectId { get; private set; }
    public Guid? RequirementId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Preconditions { get; private set; } = string.Empty;
    public string Steps { get; private set; } = string.Empty;
    public string ExpectedResult { get; private set; } = string.Empty;
    public string? ActualResult { get; private set; }
    public TestCaseStatus Status { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? ExecutedAtUtc { get; private set; }
    public void Execute(TestCaseStatus status, string? actualResult) { if (status == TestCaseStatus.NotRun) throw new ArgumentException("Execution must have a result.", nameof(status)); Status = status; ActualResult = actualResult?.Trim(); ExecutedAtUtc = DateTime.UtcNow; }
}

public sealed class Defect
{
    private Defect() { }
    public Defect(Guid projectId, Guid? requirementId, Guid? testCaseId, string title, string description, DefectSeverity severity, DefectPriority priority)
    {
        if (projectId == Guid.Empty) throw new ArgumentException("Project is required.", nameof(projectId));
        if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Defect title is required.", nameof(title));
        Id = Guid.NewGuid(); ProjectId = projectId; RequirementId = requirementId; TestCaseId = testCaseId; Title = title.Trim(); Description = description?.Trim() ?? string.Empty; Severity = severity; Priority = priority; Status = DefectStatus.Open; CreatedAtUtc = DateTime.UtcNow;
    }
    public Guid Id { get; private set; }
    public Guid ProjectId { get; private set; }
    public Guid? RequirementId { get; private set; }
    public Guid? TestCaseId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public DefectSeverity Severity { get; private set; }
    public DefectPriority Priority { get; private set; }
    public DefectStatus Status { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? UpdatedAtUtc { get; private set; }
    public void ChangeStatus(DefectStatus status) { Status = status; UpdatedAtUtc = DateTime.UtcNow; }
}

public enum RiskProbability { Low = 1, Medium = 2, High = 3 }
public enum RiskImpact { Low = 1, Medium = 2, High = 3 }
public enum RiskStatus { Open = 1, Mitigating = 2, Closed = 3, Accepted = 4 }
public enum TestCaseStatus { NotRun = 1, Passed = 2, Failed = 3, Blocked = 4 }
public enum DefectSeverity { Low = 1, Medium = 2, High = 3, Critical = 4 }
public enum DefectPriority { Low = 1, Medium = 2, High = 3, Urgent = 4 }
public enum DefectStatus { Open = 1, InProgress = 2, Resolved = 3, Closed = 4, Rejected = 5 }
