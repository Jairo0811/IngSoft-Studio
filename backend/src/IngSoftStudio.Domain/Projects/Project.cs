namespace IngSoftStudio.Domain.Projects;

public sealed class Project
{
    private Project()
    {
    }

    public Project(string name, string? description)
    {
        Id = Guid.NewGuid();
        Rename(name);
        Description = Normalize(description);
        Status = ProjectStatus.Draft;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public ProjectStatus Status { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? UpdatedAtUtc { get; private set; }

    public void Rename(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Project name is required.", nameof(name));
        }

        Name = name.Trim();
        UpdatedAtUtc = DateTime.UtcNow;
    }

    private static string? Normalize(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}

public enum ProjectStatus
{
    Draft = 1,
    Active = 2,
    Completed = 3,
    Archived = 4
}
