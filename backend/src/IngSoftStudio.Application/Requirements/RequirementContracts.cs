using System.ComponentModel.DataAnnotations;
using IngSoftStudio.Domain.Requirements;

namespace IngSoftStudio.Application.Requirements;

public sealed record CreateRequirementRequest(
    [Required, StringLength(200, MinimumLength = 2)] string Title,
    [Required, StringLength(4000, MinimumLength = 2)] string Description,
    [EnumDataType(typeof(RequirementType))] RequirementType Type,
    [EnumDataType(typeof(RequirementPriority))] RequirementPriority Priority,
    [StringLength(4000)] string? AcceptanceCriteria,
    [StringLength(500)] string? Source);

public sealed record UpdateRequirementRequest(
    [Required, StringLength(200, MinimumLength = 2)] string Title,
    [Required, StringLength(4000, MinimumLength = 2)] string Description,
    [EnumDataType(typeof(RequirementType))] RequirementType Type,
    [EnumDataType(typeof(RequirementPriority))] RequirementPriority Priority,
    [StringLength(4000)] string? AcceptanceCriteria,
    [StringLength(500)] string? Source);

public sealed record ChangeRequirementStatusRequest(
    [EnumDataType(typeof(RequirementStatus))] RequirementStatus Status);

public sealed record RequirementResponse(
    Guid Id,
    Guid ProjectId,
    string Title,
    string Description,
    RequirementType Type,
    RequirementPriority Priority,
    RequirementStatus Status,
    string? AcceptanceCriteria,
    string? Source,
    DateTime CreatedAtUtc,
    DateTime? UpdatedAtUtc);

public interface IRequirementService
{
    Task<IReadOnlyCollection<RequirementResponse>> GetByProjectAsync(Guid projectId, CancellationToken cancellationToken);
    Task<RequirementResponse?> GetByIdAsync(Guid projectId, Guid id, CancellationToken cancellationToken);
    Task<RequirementResponse> CreateAsync(Guid projectId, CreateRequirementRequest request, CancellationToken cancellationToken);
    Task<RequirementResponse?> UpdateAsync(Guid projectId, Guid id, UpdateRequirementRequest request, CancellationToken cancellationToken);
    Task<RequirementResponse?> ChangeStatusAsync(Guid projectId, Guid id, RequirementStatus status, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Guid projectId, Guid id, CancellationToken cancellationToken);
}
