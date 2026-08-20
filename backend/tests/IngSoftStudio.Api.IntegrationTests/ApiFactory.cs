using IngSoftStudio.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace IngSoftStudio.Api.IntegrationTests;

public sealed class ApiFactory : WebApplicationFactory<Program>
{
    private readonly SqliteConnection _connection = new("Data Source=:memory:");

    public ApiFactory()
    {
        _connection.Open();
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
        Environment.SetEnvironmentVariable("DOTNET_ENVIRONMENT", "Development");
        Environment.SetEnvironmentVariable(
            "ConnectionStrings__DefaultConnection",
            "Server=(localdb)\\MSSQLLocalDB;Database=IngSoftStudioTests;Trusted_Connection=True");
        Environment.SetEnvironmentVariable(
            "Frontend__AllowedOrigins__0",
            "https://localhost:5173");
        Environment.SetEnvironmentVariable("AllowedHosts", "localhost");
        Environment.SetEnvironmentVariable("Jwt__Issuer", "IngSoftStudio.Tests");
        Environment.SetEnvironmentVariable("Jwt__Audience", "IngSoftStudio.Tests");
        Environment.SetEnvironmentVariable("Jwt__SigningKey", "integration-tests-only-signing-key-1234567890");
        Environment.SetEnvironmentVariable("Database__EnsureCreated", "true");
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.RemoveAll<IDbContextOptionsConfiguration<IngSoftStudioDbContext>>();
            services.RemoveAll<DbContextOptions<IngSoftStudioDbContext>>();
            services.RemoveAll<IngSoftStudioDbContext>();

            services.AddDbContext<IngSoftStudioDbContext>(options => options.UseSqlite(_connection));
        });
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing) _connection.Dispose();
    }
}
