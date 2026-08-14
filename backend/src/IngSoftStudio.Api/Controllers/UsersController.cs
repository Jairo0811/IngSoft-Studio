using IngSoftStudio.Api.Identity;
using IngSoftStudio.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IngSoftStudio.Api.Controllers;

[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/admin/users")]
public sealed class UsersController(UserManager<ApplicationUser> userManager) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<UserResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var users = await userManager.Users
            .OrderBy(user => user.FullName)
            .ToListAsync(cancellationToken);

        var response = new List<UserResponse>(users.Count);
        foreach (var user in users)
        {
            var roles = (await userManager.GetRolesAsync(user)).ToArray();
            response.Add(new UserResponse(user.Id, user.FullName, user.Email ?? string.Empty, roles));
        }

        return Ok(response);
    }

    [HttpPut("{userId:guid}/roles/{roleName}")]
    public async Task<IActionResult> AddRole(Guid userId, string roleName)
    {
        if (roleName is not ("Admin" or "User"))
        {
            return BadRequest(new ProblemDetails { Title = "Unsupported role" });
        }

        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user is null)
        {
            return NotFound();
        }

        var result = await userManager.AddToRoleAsync(user, roleName);
        return result.Succeeded ? NoContent() : BadRequest(CreateValidationProblem(result));
    }

    [HttpDelete("{userId:guid}/roles/{roleName}")]
    public async Task<IActionResult> RemoveRole(Guid userId, string roleName)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user is null)
        {
            return NotFound();
        }

        var currentRoles = await userManager.GetRolesAsync(user);
        if (currentRoles.Count == 1 && currentRoles.Contains(roleName, StringComparer.OrdinalIgnoreCase))
        {
            return Conflict(new ProblemDetails { Title = "A user must retain at least one role." });
        }

        var result = await userManager.RemoveFromRoleAsync(user, roleName);
        return result.Succeeded ? NoContent() : BadRequest(CreateValidationProblem(result));
    }

    private static ValidationProblemDetails CreateValidationProblem(IdentityResult result) =>
        new(result.Errors
            .GroupBy(error => error.Code)
            .ToDictionary(group => group.Key, group => group.Select(error => error.Description).ToArray()));
}
