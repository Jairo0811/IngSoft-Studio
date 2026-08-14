using IngSoftStudio.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace IngSoftStudio.Api.IntegrationTests;

public sealed class ApiFactory : WebApplicationFactory<Program>
{
    public ApiFactory()
    {
        Environment.SetEnvironmentVariable("Jwt__Issuer", "IngSoftStudio.Tests");
        Environment.SetEnvironmentVariable("Jwt__Audience", "IngSoftStudio.Tests");
        Environment.SetEnvironmentVariable("Jwt__SigningKey", "integration-tests-only-signing-key-1234567890");
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.RemoveAll<IDatabaseProvider>();
            services.RemoveAll<DbContextOptions<IngSoftStudioDbContext>>();
            services.RemoveAll<IngSoftStudioDbContext>();
            services.AddDbContext<IngSoftStudioDbContext>(options =>
                options.UseInMemoryDatabase($"IngSoftStudioIntegrationTests-{Guid.NewGuid()}"));
        });
    }
}
