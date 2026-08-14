using IngSoftStudio.Application.Projects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IngSoftStudio.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/projects")]
public sealed class ProjectsController(IProjectService projectService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<ProjectResponse>>> GetAll(CancellationToken cancellationToken) => Ok(await projectService.GetAllAsync(cancellationToken));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProjectResponse>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var project = await projectService.GetByIdAsync(id, cancellationToken);
        return project is null ? NotFound() : Ok(project);
    }

    [HttpPost]
    public async Task<ActionResult<ProjectResponse>> Create(CreateProjectRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name)) return ValidationProblem();
        var project = await projectService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = project.Id }, project);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ProjectResponse>> Update(Guid id, UpdateProjectRequest request, CancellationToken cancellationToken)
    {
        var project = await projectService.UpdateAsync(id, request, cancellationToken);
        return project is null ? NotFound() : Ok(project);
    }

    [HttpPatch("{id:guid}/status")]
    public async Task<ActionResult<ProjectResponse>> ChangeStatus(Guid id, ChangeProjectStatusRequest request, CancellationToken cancellationToken)
    {
        var project = await projectService.ChangeStatusAsync(id, request.Status, cancellationToken);
        return project is null ? NotFound() : Ok(project);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken) => await projectService.DeleteAsync(id, cancellationToken) ? NoContent() : NotFound();
}
