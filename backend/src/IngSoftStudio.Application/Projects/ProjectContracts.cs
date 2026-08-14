using IngSoftStudio.Domain.Projects;

namespace IngSoftStudio.Application.Projects;

public sealed record CreateProjectRequest(string Name, string? Description);
public sealed record UpdateProjectRequest(string Name, string? Description);
public sealed record ChangeProjectStatusRequest(ProjectStatus Status);
public sealed record ProjectResponse(Guid Id, string Name, string? Description, string Status, DateTime CreatedAtUtc, DateTime? UpdatedAtUtc);

public interface IProjectService
{
    Task<IReadOnlyCollection<ProjectResponse>> GetAllAsync(CancellationToken cancellationToken);
    Task<ProjectResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<ProjectResponse> CreateAsync(CreateProjectRequest request, CancellationToken cancellationToken);
    Task<ProjectResponse?> UpdateAsync(Guid id, UpdateProjectRequest request, CancellationToken cancellationToken);
    Task<ProjectResponse?> ChangeStatusAsync(Guid id, ProjectStatus status, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
}
