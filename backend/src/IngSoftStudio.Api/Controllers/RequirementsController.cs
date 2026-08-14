using IngSoftStudio.Application.Requirements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IngSoftStudio.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/projects/{projectId:guid}/requirements")]
public sealed class RequirementsController(IRequirementService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<RequirementResponse>>> GetAll(Guid projectId, CancellationToken cancellationToken) =>
        Ok(await service.GetByProjectAsync(projectId, cancellationToken));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RequirementResponse>> GetById(Guid projectId, Guid id, CancellationToken cancellationToken)
    {
        var requirement = await service.GetByIdAsync(projectId, id, cancellationToken);
        return requirement is null ? NotFound() : Ok(requirement);
    }

    [HttpPost]
    public async Task<ActionResult<RequirementResponse>> Create(Guid projectId, CreateRequirementRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Title) || string.IsNullOrWhiteSpace(request.Description)) return ValidationProblem();
        var requirement = await service.CreateAsync(projectId, request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { projectId, id = requirement.Id }, requirement);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<RequirementResponse>> Update(Guid projectId, Guid id, UpdateRequirementRequest request, CancellationToken cancellationToken)
    {
        var requirement = await service.UpdateAsync(projectId, id, request, cancellationToken);
        return requirement is null ? NotFound() : Ok(requirement);
    }

    [HttpPatch("{id:guid}/status")]
    public async Task<ActionResult<RequirementResponse>> ChangeStatus(Guid projectId, Guid id, ChangeRequirementStatusRequest request, CancellationToken cancellationToken)
    {
        var requirement = await service.ChangeStatusAsync(projectId, id, request.Status, cancellationToken);
        return requirement is null ? NotFound() : Ok(requirement);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid projectId, Guid id, CancellationToken cancellationToken) =>
        await service.DeleteAsync(projectId, id, cancellationToken) ? NoContent() : NotFound();
}
