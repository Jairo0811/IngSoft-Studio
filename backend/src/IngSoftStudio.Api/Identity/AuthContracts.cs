using System.ComponentModel.DataAnnotations;

namespace IngSoftStudio.Api.Identity;

public sealed record RegisterRequest(
    [property: Required, StringLength(150, MinimumLength = 2)] string FullName,
    [property: Required, EmailAddress, StringLength(256)] string Email,
    [property: Required, StringLength(128, MinimumLength = 12)] string Password);

public sealed record LoginRequest(
    [property: Required, EmailAddress, StringLength(256)] string Email,
    [property: Required, StringLength(128, MinimumLength = 1)] string Password);

public sealed record ForgotPasswordRequest(
    [property: Required, EmailAddress, StringLength(256)] string Email);

public sealed record ResetPasswordRequest(
    [property: Required, EmailAddress, StringLength(256)] string Email,
    [property: Required, StringLength(4096, MinimumLength = 8)] string Token,
    [property: Required, StringLength(128, MinimumLength = 12)] string NewPassword);

public sealed record ChangePasswordRequest(
    [property: Required, StringLength(128, MinimumLength = 1)] string CurrentPassword,
    [property: Required, StringLength(128, MinimumLength = 12)] string NewPassword);

public sealed record UpdateProfileRequest(
    [property: Required, StringLength(150, MinimumLength = 2)] string FullName);

public sealed record AuthResponse(
    string AccessToken,
    DateTime ExpiresAtUtc,
    UserResponse User);

public sealed record UserResponse(
    Guid Id,
    string FullName,
    string Email,
    IReadOnlyCollection<string> Roles);
