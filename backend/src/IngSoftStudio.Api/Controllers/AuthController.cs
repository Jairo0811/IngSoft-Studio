using System.Security.Claims;
using IngSoftStudio.Api.Identity;
using IngSoftStudio.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IngSoftStudio.Api.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    JwtTokenService jwtTokenService,
    IWebHostEnvironment environment) : ControllerBase
{
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request)
    {
        var existingUser = await userManager.FindByEmailAsync(request.Email.Trim());
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
            UserName = request.Email.Trim(),
            Email = request.Email.Trim(),
            FullName = request.FullName.Trim(),
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            return ValidationProblem(ToValidationDictionary(result));
        }

        await userManager.AddToRoleAsync(user, "User");
        return Ok(await jwtTokenService.CreateAsync(user));
    }

    [HttpPost("login")]
    [AllowAnonymous]
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

        var roles = await userManager.GetRolesAsync(user);
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

        user.FullName = request.FullName.Trim();
        var result = await userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            return ValidationProblem(ToValidationDictionary(result));
        }

        var roles = await userManager.GetRolesAsync(user);
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
        return result.Succeeded ? NoContent() : ValidationProblem(ToValidationDictionary(result));
    }

    [HttpPost("forgot-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.Email.Trim());
        if (user is null)
        {
            return Accepted();
        }

        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        if (environment.IsDevelopment())
        {
            return Accepted(new { resetToken = token });
        }

        return Accepted();
    }

    [HttpPost("reset-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.Email.Trim());
        if (user is null)
        {
            return NoContent();
        }

        var result = await userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
        return result.Succeeded ? NoContent() : ValidationProblem(ToValidationDictionary(result));
    }

    private async Task<ApplicationUser?> GetCurrentUserAsync()
    {
        var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(id, out var userId) ? await userManager.FindByIdAsync(userId.ToString()) : null;
    }

    private static Dictionary<string, string[]> ToValidationDictionary(IdentityResult result) =>
        result.Errors
            .GroupBy(error => error.Code)
            .ToDictionary(group => group.Key, group => group.Select(error => error.Description).ToArray());
}
