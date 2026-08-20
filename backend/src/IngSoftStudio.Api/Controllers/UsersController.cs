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
        var normalizedRole = NormalizeRole(roleName);
        if (normalizedRole is null)
        {
            return BadRequest(new ProblemDetails { Title = "Unsupported role" });
        }

        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user is null)
        {
            return NotFound();
        }

        var result = await userManager.AddToRoleAsync(user, normalizedRole);
        if (!result.Succeeded)
        {
            return BadRequest(CreateValidationProblem(result));
        }

        await userManager.UpdateSecurityStampAsync(user);
        return NoContent();
    }

    [HttpDelete("{userId:guid}/roles/{roleName}")]
    public async Task<IActionResult> RemoveRole(Guid userId, string roleName)
    {
        var normalizedRole = NormalizeRole(roleName);
        if (normalizedRole is null)
        {
            return BadRequest(new ProblemDetails { Title = "Unsupported role" });
        }

        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user is null)
        {
            return NotFound();
        }

        var currentRoles = await userManager.GetRolesAsync(user);
        if (currentRoles.Count == 1 && currentRoles.Contains(normalizedRole, StringComparer.OrdinalIgnoreCase))
        {
            return Conflict(new ProblemDetails { Title = "A user must retain at least one role." });
        }

        if (normalizedRole == "Admin" && currentRoles.Contains("Admin", StringComparer.OrdinalIgnoreCase))
        {
            var admins = await userManager.GetUsersInRoleAsync("Admin");
            if (admins.Count <= 1)
            {
                return Conflict(new ProblemDetails
                {
                    Title = "The last administrator cannot lose the Admin role."
                });
            }
        }

        var result = await userManager.RemoveFromRoleAsync(user, normalizedRole);
        if (!result.Succeeded)
        {
            return BadRequest(CreateValidationProblem(result));
        }

        await userManager.UpdateSecurityStampAsync(user);
        return NoContent();
    }

    private static string? NormalizeRole(string roleName) =>
        roleName.Equals("Admin", StringComparison.OrdinalIgnoreCase)
            ? "Admin"
            : roleName.Equals("User", StringComparison.OrdinalIgnoreCase)
                ? "User"
                : null;

    private static ValidationProblemDetails CreateValidationProblem(IdentityResult result) =>
        new(result.Errors
            .GroupBy(error => error.Code)
            .ToDictionary(group => group.Key, group => group.Select(error => error.Description).ToArray()));
}
