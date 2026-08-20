using System.Security.Claims;
using IngSoftStudio.Api.Identity;
using IngSoftStudio.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace IngSoftStudio.Api.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    JwtTokenService jwtTokenService) : ControllerBase
{
    [HttpPost("register")]
    [AllowAnonymous]
    [EnableRateLimiting("auth")]
    public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request)
    {
        var email = request.Email.Trim();
        var fullName = request.FullName.Trim();

        if (string.IsNullOrWhiteSpace(fullName))
        {
            return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]>
            {
                [nameof(request.FullName)] = ["Full name is required."]
            }));
        }

        var existingUser = await userManager.FindByEmailAsync(email);
        if (existingUser is not null)
        {
            return Conflict(new ProblemDetails
            {
                Title = "Email already registered",
                Detail = "An account already exists for the supplied email address.",
                Status = StatusCodes.Status409Conflict
            });
        }

        var user = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            UserName = email,
            Email = email,
            FullName = fullName,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            return BadRequest(CreateValidationProblem(result));
        }

        var roleResult = await userManager.AddToRoleAsync(user, "User");
        if (!roleResult.Succeeded)
        {
            await userManager.DeleteAsync(user);
            return BadRequest(CreateValidationProblem(roleResult));
        }

        return Ok(await jwtTokenService.CreateAsync(user));
    }

    [HttpPost("login")]
    [AllowAnonymous]
    [EnableRateLimiting("auth")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.Email.Trim());
        if (user is null)
        {
            return Unauthorized();
        }

        var result = await signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: true);
        if (!result.Succeeded)
        {
            return Unauthorized();
        }

        return Ok(await jwtTokenService.CreateAsync(user));
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<UserResponse>> Me()
    {
        var user = await GetCurrentUserAsync();
        if (user is null)
        {
            return Unauthorized();
        }

        var roles = (await userManager.GetRolesAsync(user)).ToArray();
        return Ok(new UserResponse(user.Id, user.FullName, user.Email ?? string.Empty, roles));
    }

    [HttpPut("profile")]
    [Authorize]
    public async Task<ActionResult<UserResponse>> UpdateProfile(UpdateProfileRequest request)
    {
        var user = await GetCurrentUserAsync();
        if (user is null)
        {
            return Unauthorized();
        }

        var fullName = request.FullName.Trim();
        if (string.IsNullOrWhiteSpace(fullName))
        {
            return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]>
            {
                [nameof(request.FullName)] = ["Full name is required."]
            }));
        }

        user.FullName = fullName;
        var result = await userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            return BadRequest(CreateValidationProblem(result));
        }

        var roles = (await userManager.GetRolesAsync(user)).ToArray();
        return Ok(new UserResponse(user.Id, user.FullName, user.Email ?? string.Empty, roles));
    }

    [HttpPost("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
    {
        var user = await GetCurrentUserAsync();
        if (user is null)
        {
            return Unauthorized();
        }

        var result = await userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
        return result.Succeeded ? NoContent() : BadRequest(CreateValidationProblem(result));
    }

    [HttpPost("forgot-password")]
    [AllowAnonymous]
    [EnableRateLimiting("auth")]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.Email.Trim());
        if (user is null)
        {
            return Accepted();
        }

        // The reset token must be delivered through a separately configured,
        // verified out-of-band channel. It is never returned or logged by the API.
        _ = await userManager.GeneratePasswordResetTokenAsync(user);
        return Accepted();
    }

    [HttpPost("reset-password")]
    [AllowAnonymous]
    [EnableRateLimiting("auth")]
    public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.Email.Trim());
        if (user is null)
        {
            return NoContent();
        }

        var result = await userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
        return result.Succeeded ? NoContent() : BadRequest(CreateValidationProblem(result));
    }

    private async Task<ApplicationUser?> GetCurrentUserAsync()
    {
        var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(id, out var userId) ? await userManager.FindByIdAsync(userId.ToString()) : null;
    }

    private static ValidationProblemDetails CreateValidationProblem(IdentityResult result) =>
        new(result.Errors
            .GroupBy(error => error.Code)
            .ToDictionary(group => group.Key, group => group.Select(error => error.Description).ToArray()));
}
