using IngSoftStudio.Domain.Requirements;

namespace IngSoftStudio.Application.Requirements;

public sealed record CreateRequirementRequest(string Title, string Description, RequirementType Type, RequirementPriority Priority);
public sealed record UpdateRequirementRequest(string Title, string Description, RequirementType Type, RequirementPriority Priority);
public sealed record RequirementResponse(Guid Id, Guid ProjectId, string Title, string Description, RequirementType Type, RequirementPriority Priority, RequirementStatus Status, DateTime CreatedAtUtc, DateTime? UpdatedAtUtc);

public interface IRequirementService
{
    Task<IReadOnlyCollection<RequirementResponse>> GetByProjectAsync(Guid projectId, CancellationToken cancellationToken);
    Task<RequirementResponse?> GetByIdAsync(Guid projectId, Guid id, CancellationToken cancellationToken);
    Task<RequirementResponse> CreateAsync(Guid projectId, CreateRequirementRequest request, CancellationToken cancellationToken);
    Task<RequirementResponse?> UpdateAsync(Guid projectId, Guid id, UpdateRequirementRequest request, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Guid projectId, Guid id, CancellationToken cancellationToken);
}
