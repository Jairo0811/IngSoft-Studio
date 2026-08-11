using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IngSoftStudio.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace IngSoftStudio.Api.Identity;

public sealed class JwtTokenService(IOptions<JwtOptions> options, UserManager<ApplicationUser> userManager)
{
    private readonly JwtOptions _options = options.Value;

    public async Task<AuthResponse> CreateAsync(ApplicationUser user)
    {
        var roles = await userManager.GetRolesAsync(user);
        var roleArray = roles.ToArray();
        var expiresAtUtc = DateTime.UtcNow.AddMinutes(_options.ExpirationMinutes);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.FullName)
        };

        claims.AddRange(roleArray.Select(role => new Claim(ClaimTypes.Role, role)));

        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SigningKey)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            expires: expiresAtUtc,
            signingCredentials: credentials);

        return new AuthResponse(
            new JwtSecurityTokenHandler().WriteToken(token),
            expiresAtUtc,
            new UserResponse(user.Id, user.FullName, user.Email ?? string.Empty, roleArray));
    }
}
