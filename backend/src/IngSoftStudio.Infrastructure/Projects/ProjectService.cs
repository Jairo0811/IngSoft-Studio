using IngSoftStudio.Application.Common;
using IngSoftStudio.Application.Projects;
using IngSoftStudio.Domain.Projects;
using IngSoftStudio.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace IngSoftStudio.Infrastructure.Projects;

public sealed class ProjectService(
    IngSoftStudioDbContext dbContext,
    ICurrentUserContext currentUser) : IProjectService
{
    public async Task<IReadOnlyCollection<ProjectResponse>> GetAllAsync(CancellationToken cancellationToken) =>
        await AccessibleProjects().AsNoTracking().OrderByDescending(x => x.CreatedAtUtc).Select(x => new ProjectResponse(x.Id, x.Name, x.Description, x.Status.ToString(), x.CreatedAtUtc, x.UpdatedAtUtc)).ToListAsync(cancellationToken);

    public async Task<ProjectResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        await AccessibleProjects().AsNoTracking().Where(x => x.Id == id).Select(x => new ProjectResponse(x.Id, x.Name, x.Description, x.Status.ToString(), x.CreatedAtUtc, x.UpdatedAtUtc)).SingleOrDefaultAsync(cancellationToken);

    public async Task<ProjectResponse> CreateAsync(CreateProjectRequest request, CancellationToken cancellationToken)
    {
        var project = new Project(currentUser.UserId, request.Name, request.Description);
        dbContext.Projects.Add(project);
        await dbContext.SaveChangesAsync(cancellationToken);
        return Map(project);
    }

    public async Task<ProjectResponse?> UpdateAsync(Guid id, UpdateProjectRequest request, CancellationToken cancellationToken)
    {
        var project = await AccessibleProjects().SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (project is null) return null;
        project.Update(request.Name, request.Description);
        await dbContext.SaveChangesAsync(cancellationToken);
        return Map(project);
    }

    public async Task<ProjectResponse?> ChangeStatusAsync(Guid id, ProjectStatus status, CancellationToken cancellationToken)
    {
        var project = await AccessibleProjects().SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (project is null) return null;
        project.ChangeStatus(status);
        await dbContext.SaveChangesAsync(cancellationToken);
        return Map(project);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var project = await AccessibleProjects().SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (project is null) return false;
        dbContext.Projects.Remove(project);
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    private static ProjectResponse Map(Project x) => new(x.Id, x.Name, x.Description, x.Status.ToString(), x.CreatedAtUtc, x.UpdatedAtUtc);

    private IQueryable<Project> AccessibleProjects() =>
        currentUser.IsAdmin
            ? dbContext.Projects
            : dbContext.Projects.Where(project => project.OwnerId == currentUser.UserId);
}
