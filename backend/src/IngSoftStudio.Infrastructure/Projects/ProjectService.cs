using IngSoftStudio.Application.Projects;
using IngSoftStudio.Domain.Projects;
using IngSoftStudio.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace IngSoftStudio.Infrastructure.Projects;

public sealed class ProjectService(IngSoftStudioDbContext dbContext) : IProjectService
{
    public async Task<IReadOnlyCollection<ProjectResponse>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await dbContext.Projects
            .AsNoTracking()
            .OrderByDescending(project => project.CreatedAtUtc)
            .Select(project => new ProjectResponse(project.Id, project.Name, project.Description, project.Status.ToString(), project.CreatedAtUtc))
            .ToListAsync(cancellationToken);
    }

    public async Task<ProjectResponse> CreateAsync(CreateProjectRequest request, CancellationToken cancellationToken)
    {
        var project = new Project(request.Name, request.Description);
        dbContext.Projects.Add(project);
        await dbContext.SaveChangesAsync(cancellationToken);
        return new ProjectResponse(project.Id, project.Name, project.Description, project.Status.ToString(), project.CreatedAtUtc);
    }
}
