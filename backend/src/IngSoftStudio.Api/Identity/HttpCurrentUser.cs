using System.Security.Claims;
using IngSoftStudio.Application.Common;

namespace IngSoftStudio.Api.Identity;

public sealed class HttpCurrentUser(IHttpContextAccessor accessor) : ICurrentUser
{
    public Guid UserId => Guid.TryParse(
        accessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier),
        out var userId)
            ? userId
            : throw new UnauthorizedAccessException("Authenticated user identifier is missing.");
}
