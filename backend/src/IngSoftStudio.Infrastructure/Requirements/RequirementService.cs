using IngSoftStudio.Application.Requirements;
using IngSoftStudio.Application.Common;
using IngSoftStudio.Domain.Requirements;
using IngSoftStudio.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace IngSoftStudio.Infrastructure.Requirements;

public sealed class RequirementService(IngSoftStudioDbContext dbContext, ICurrentUser currentUser) : IRequirementService
{
    public async Task<IReadOnlyCollection<RequirementResponse>> GetByProjectAsync(Guid projectId, CancellationToken cancellationToken) =>
        await OwnsProjectAsync(projectId, cancellationToken)
            ? await dbContext.Requirements.AsNoTracking().Where(x => x.ProjectId == projectId).OrderBy(x => x.Priority).ThenBy(x => x.CreatedAtUtc).Select(MapExpression).ToListAsync(cancellationToken)
            : [];

    public async Task<RequirementResponse?> GetByIdAsync(Guid projectId, Guid id, CancellationToken cancellationToken) =>
        await dbContext.Requirements.AsNoTracking().Where(x => x.ProjectId == projectId && x.Id == id && dbContext.Projects.Any(p => p.Id == projectId && p.OwnerId == currentUser.UserId)).Select(MapExpression).SingleOrDefaultAsync(cancellationToken);

    public async Task<RequirementResponse> CreateAsync(Guid projectId, CreateRequirementRequest request, CancellationToken cancellationToken)
    {
        if (!await OwnsProjectAsync(projectId, cancellationToken)) throw new KeyNotFoundException("Project not found.");
        var requirement = new Requirement(projectId, request.Title, request.Description, request.Type, request.Priority, request.AcceptanceCriteria, request.Source);
        dbContext.Requirements.Add(requirement);
        await dbContext.SaveChangesAsync(cancellationToken);
        return Map(requirement);
    }

    public async Task<RequirementResponse?> UpdateAsync(Guid projectId, Guid id, UpdateRequirementRequest request, CancellationToken cancellationToken)
    {
        if (!await OwnsProjectAsync(projectId, cancellationToken)) return null;
        var requirement = await dbContext.Requirements.SingleOrDefaultAsync(x => x.ProjectId == projectId && x.Id == id, cancellationToken);
        if (requirement is null) return null;
        requirement.Update(request.Title, request.Description, request.Type, request.Priority, request.AcceptanceCriteria, request.Source);
        await dbContext.SaveChangesAsync(cancellationToken);
        return Map(requirement);
    }

    public async Task<RequirementResponse?> ChangeStatusAsync(Guid projectId, Guid id, RequirementStatus status, CancellationToken cancellationToken)
    {
        if (!await OwnsProjectAsync(projectId, cancellationToken)) return null;
        var requirement = await dbContext.Requirements.SingleOrDefaultAsync(x => x.ProjectId == projectId && x.Id == id, cancellationToken);
        if (requirement is null) return null;
        requirement.ChangeStatus(status);
        await dbContext.SaveChangesAsync(cancellationToken);
        return Map(requirement);
    }

    public async Task<bool> DeleteAsync(Guid projectId, Guid id, CancellationToken cancellationToken)
    {
        if (!await OwnsProjectAsync(projectId, cancellationToken)) return false;
        var requirement = await dbContext.Requirements.SingleOrDefaultAsync(x => x.ProjectId == projectId && x.Id == id, cancellationToken);
        if (requirement is null) return false;
        dbContext.Requirements.Remove(requirement);
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    private static readonly System.Linq.Expressions.Expression<Func<Requirement, RequirementResponse>> MapExpression = x =>
        new(x.Id, x.ProjectId, x.Title, x.Description, x.Type, x.Priority, x.Status, x.AcceptanceCriteria, x.Source, x.CreatedAtUtc, x.UpdatedAtUtc);

    private static RequirementResponse Map(Requirement x) => new(x.Id, x.ProjectId, x.Title, x.Description, x.Type, x.Priority, x.Status, x.AcceptanceCriteria, x.Source, x.CreatedAtUtc, x.UpdatedAtUtc);

    private Task<bool> OwnsProjectAsync(Guid projectId, CancellationToken cancellationToken) =>
        dbContext.Projects.AnyAsync(x => x.Id == projectId && x.OwnerId == currentUser.UserId, cancellationToken);
}
