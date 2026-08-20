using System.Security.Claims;
using IngSoftStudio.Application.Common;

namespace IngSoftStudio.Api.Identity;

public sealed class HttpCurrentUserContext(IHttpContextAccessor httpContextAccessor)
    : ICurrentUserContext
{
    private ClaimsPrincipal User =>
        httpContextAccessor.HttpContext?.User
        ?? throw new UnauthorizedAccessException("No authenticated user is available.");

    public Guid UserId
    {
        get
        {
            var value = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(value, out var userId)
                ? userId
                : throw new UnauthorizedAccessException("The authenticated user identifier is invalid.");
        }
    }

    public bool IsAdmin => User.IsInRole("Admin");
}
