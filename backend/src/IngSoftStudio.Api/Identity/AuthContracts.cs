using System.ComponentModel.DataAnnotations;

namespace IngSoftStudio.Api.Identity;

public sealed record RegisterRequest(
    [Required, StringLength(150, MinimumLength = 2)] string FullName,
    [Required, EmailAddress, StringLength(256)] string Email,
    [Required, StringLength(128, MinimumLength = 12)] string Password);

public sealed record LoginRequest(
    [Required, EmailAddress, StringLength(256)] string Email,
    [Required, StringLength(128, MinimumLength = 1)] string Password);

public sealed record ForgotPasswordRequest(
    [Required, EmailAddress, StringLength(256)] string Email);

public sealed record ResetPasswordRequest(
    [Required, EmailAddress, StringLength(256)] string Email,
    [Required, StringLength(4096, MinimumLength = 8)] string Token,
    [Required, StringLength(128, MinimumLength = 12)] string NewPassword);

public sealed record ChangePasswordRequest(
    [Required, StringLength(128, MinimumLength = 1)] string CurrentPassword,
    [Required, StringLength(128, MinimumLength = 12)] string NewPassword);

public sealed record UpdateProfileRequest(
    [Required, StringLength(150, MinimumLength = 2)] string FullName);

public sealed record AuthResponse(
    string AccessToken,
    DateTime ExpiresAtUtc,
    UserResponse User);

public sealed record UserResponse(
    Guid Id,
    string FullName,
    string Email,
    IReadOnlyCollection<string> Roles);
