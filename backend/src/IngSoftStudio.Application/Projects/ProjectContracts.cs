namespace IngSoftStudio.Application.Projects;

public sealed record CreateProjectRequest(string Name, string? Description);

public sealed record ProjectResponse(
    Guid Id,
    string Name,
    string? Description,
    string Status,
    DateTime CreatedAtUtc);

public interface IProjectService
{
    Task<IReadOnlyCollection<ProjectResponse>> GetAllAsync(CancellationToken cancellationToken);
    Task<ProjectResponse> CreateAsync(CreateProjectRequest request, CancellationToken cancellationToken);
}
