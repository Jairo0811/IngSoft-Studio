using System.Security.Cryptography;
using System.Text;

namespace IngSoftStudio.Api.Identity;

public static class SecurityStampHasher
{
    public static string Hash(string? securityStamp)
    {
        var bytes = Encoding.UTF8.GetBytes(securityStamp ?? string.Empty);
        return Convert.ToHexString(SHA256.HashData(bytes));
    }
}
