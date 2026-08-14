using IngSoftStudio.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace IngSoftStudio.Api.IntegrationTests;

public sealed class ApiFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((_, configuration) =>
        {
            configuration.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Issuer"] = "IngSoftStudio.Tests",
                ["Jwt:Audience"] = "IngSoftStudio.Tests",
                ["Jwt:SigningKey"] = "integration-tests-only-signing-key-1234567890"
            });
        });

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<DbContextOptions<IngSoftStudioDbContext>>();
            services.RemoveAll<IngSoftStudioDbContext>();
            services.AddDbContext<IngSoftStudioDbContext>(options =>
                options.UseInMemoryDatabase($"IngSoftStudioIntegrationTests-{Guid.NewGuid()}"));
        });
    }
}
