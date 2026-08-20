namespace IngSoftStudio.Domain.Projects;

public sealed class Project
{
    private Project() { }

    public Project(Guid ownerId, string name, string? description)
    {
        if (ownerId == Guid.Empty)
        {
            throw new ArgumentException("Project owner is required.", nameof(ownerId));
        }

        Id = Guid.NewGuid();
        OwnerId = ownerId;
        Update(name, description);
        Status = ProjectStatus.Draft;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }
    public Guid? OwnerId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public ProjectStatus Status { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? UpdatedAtUtc { get; private set; }

    public void Update(string name, string? description)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Project name is required.", nameof(name));
        Name = name.Trim();
        Description = Normalize(description);
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void Rename(string name) => Update(name, Description);

    public void ChangeStatus(ProjectStatus status)
    {
        Status = status;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    private static string? Normalize(string? value) => string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}

public enum ProjectStatus { Draft = 1, Active = 2, Completed = 3, Archived = 4 }
