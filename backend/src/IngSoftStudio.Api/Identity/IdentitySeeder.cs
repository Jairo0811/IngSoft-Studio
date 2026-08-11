using IngSoftStudio.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace IngSoftStudio.Api.Identity;

public static class IdentitySeeder
{
    private static readonly string[] DefaultRoles = ["Admin", "User"];

    public static async Task SeedAsync(IServiceProvider services, IConfiguration configuration)
    {
        using var scope = services.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        foreach (var roleName in DefaultRoles)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                var roleResult = await roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
                if (!roleResult.Succeeded)
                {
                    throw new InvalidOperationException($"Unable to seed role '{roleName}'.");
                }
            }
        }

        var email = configuration["SeedAdmin:Email"]?.Trim();
        var password = configuration["SeedAdmin:Password"];
        var fullName = configuration["SeedAdmin:FullName"]?.Trim();

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(fullName))
        {
            return;
        }

        var admin = await userManager.FindByEmailAsync(email);
        if (admin is null)
        {
            admin = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                UserName = email,
                Email = email,
                FullName = fullName,
                EmailConfirmed = true
            };

            var createResult = await userManager.CreateAsync(admin, password);
            if (!createResult.Succeeded)
            {
                throw new InvalidOperationException("Unable to create the configured seed administrator.");
            }
        }

        foreach (var roleName in DefaultRoles)
        {
            if (!await userManager.IsInRoleAsync(admin, roleName))
            {
                var addRoleResult = await userManager.AddToRoleAsync(admin, roleName);
                if (!addRoleResult.Succeeded)
                {
                    throw new InvalidOperationException($"Unable to assign role '{roleName}' to the seed administrator.");
                }
            }
        }
    }
}
