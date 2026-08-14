namespace IngSoftStudio.Domain.Requirements;

public sealed class Requirement
{
    private Requirement() { }

    public Requirement(Guid projectId, string title, string description, RequirementType type, RequirementPriority priority, string? acceptanceCriteria = null, string? source = null)
    {
        if (projectId == Guid.Empty) throw new ArgumentException("Project is required.", nameof(projectId));
        Id = Guid.NewGuid();
        ProjectId = projectId;
        Update(title, description, type, priority, acceptanceCriteria, source);
        Status = RequirementStatus.Proposed;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }
    public Guid ProjectId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public RequirementType Type { get; private set; }
    public RequirementPriority Priority { get; private set; }
    public RequirementStatus Status { get; private set; }
    public string? AcceptanceCriteria { get; private set; }
    public string? Source { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? UpdatedAtUtc { get; private set; }

    public void Update(string title, string description, RequirementType type, RequirementPriority priority, string? acceptanceCriteria, string? source)
    {
        if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Requirement title is required.", nameof(title));
        if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException("Requirement description is required.", nameof(description));
        Title = title.Trim();
        Description = description.Trim();
        Type = type;
        Priority = priority;
        AcceptanceCriteria = Normalize(acceptanceCriteria);
        Source = Normalize(source);
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void ChangeStatus(RequirementStatus status)
    {
        Status = status;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    private static string? Normalize(string? value) => string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}

public enum RequirementType { Functional = 1, NonFunctional = 2, UserStory = 3, UseCase = 4 }
public enum RequirementPriority { Must = 1, Should = 2, Could = 3, Wont = 4 }
public enum RequirementStatus { Proposed = 1, Approved = 2, InProgress = 3, Implemented = 4, Rejected = 5 }
