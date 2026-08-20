using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace IngSoftStudio.Api.IntegrationTests;

public sealed class Phase3WorkflowTests(ApiFactory factory) : IClassFixture<ApiFactory>
{
    [Fact]
    public async Task UserCannotAccessOrModifyAnotherUsersProject()
    {
        using var ownerClient = factory.CreateClient();
        using var attackerClient = factory.CreateClient();
        var cancellationToken = TestContext.Current.CancellationToken;
        ownerClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", await RegisterAndGetToken(ownerClient, cancellationToken));
        attackerClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", await RegisterAndGetToken(attackerClient, cancellationToken));

        var created = await ownerClient.PostAsJsonAsync(
            "/api/v1/projects",
            new { name = "Proyecto privado", description = "Solo del propietario" },
            cancellationToken);
        created.EnsureSuccessStatusCode();
        using var projectJson = JsonDocument.Parse(await created.Content.ReadAsStringAsync(cancellationToken));
        var projectId = projectJson.RootElement.GetProperty("id").GetGuid();

        Assert.Equal(HttpStatusCode.NotFound,
            (await attackerClient.GetAsync($"/api/v1/projects/{projectId}", cancellationToken)).StatusCode);
        Assert.Equal(HttpStatusCode.NotFound,
            (await attackerClient.PutAsJsonAsync($"/api/v1/projects/{projectId}", new { name = "Secuestrado", description = "IDOR" }, cancellationToken)).StatusCode);
        Assert.Equal(HttpStatusCode.NotFound,
            (await attackerClient.DeleteAsync($"/api/v1/projects/{projectId}", cancellationToken)).StatusCode);
        Assert.Equal(HttpStatusCode.NotFound,
            (await attackerClient.PostAsJsonAsync(
                $"/api/v1/projects/{projectId}/requirements",
                new { title = "Ataque", description = "IDOR", type = 1, priority = 1 },
                cancellationToken)).StatusCode);
        Assert.Equal(HttpStatusCode.NotFound,
            (await attackerClient.GetAsync($"/api/v1/projects/{projectId}/quality", cancellationToken)).StatusCode);

        using var attackerList = JsonDocument.Parse(await attackerClient.GetStringAsync("/api/v1/projects", cancellationToken));
        Assert.DoesNotContain(attackerList.RootElement.EnumerateArray(), item => item.GetProperty("id").GetGuid() == projectId);
    }

    [Fact]
    public async Task AuthenticatedUserCanCreateProjectAndRequirement()
    {
        using var client = factory.CreateClient();
        var cancellationToken = TestContext.Current.CancellationToken;
        var email = $"phase3-{Guid.NewGuid():N}@tests.local";

        var registerResponse = await client.PostAsJsonAsync(
            "/api/auth/register",
            new { fullName = "Phase 3 Test", email, password = "StrongPass123!" },
            cancellationToken);
        Assert.Equal(HttpStatusCode.OK, registerResponse.StatusCode);

        using var registerJson = JsonDocument.Parse(await registerResponse.Content.ReadAsStringAsync(cancellationToken));
        var accessToken = registerJson.RootElement.GetProperty("accessToken").GetString();
        Assert.False(string.IsNullOrWhiteSpace(accessToken));
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var projectResponse = await client.PostAsJsonAsync(
            "/api/v1/projects",
            new { name = "Proyecto de integración", description = "Validación del flujo de Fase 3" },
            cancellationToken);
        Assert.Equal(HttpStatusCode.Created, projectResponse.StatusCode);

        using var projectJson = JsonDocument.Parse(await projectResponse.Content.ReadAsStringAsync(cancellationToken));
        var projectId = projectJson.RootElement.GetProperty("id").GetGuid();

        var requirementResponse = await client.PostAsJsonAsync(
            $"/api/v1/projects/{projectId}/requirements",
            new
            {
                title = "Autenticación obligatoria",
                description = "El sistema debe proteger el workspace.",
                type = 1,
                priority = 1,
                acceptanceCriteria = "Sin token se rechaza el acceso.",
                source = "Fase 3"
            },
            cancellationToken);

        Assert.Equal(HttpStatusCode.Created, requirementResponse.StatusCode);

        var listResponse = await client.GetAsync($"/api/v1/projects/{projectId}/requirements", cancellationToken);
        Assert.Equal(HttpStatusCode.OK, listResponse.StatusCode);

        using var listJson = JsonDocument.Parse(await listResponse.Content.ReadAsStringAsync(cancellationToken));
        Assert.Single(listJson.RootElement.EnumerateArray());
    }

    private static async Task<string> RegisterAndGetToken(HttpClient client, CancellationToken cancellationToken)
    {
        var response = await client.PostAsJsonAsync(
            "/api/auth/register",
            new { fullName = "IDOR Test User", email = $"idor-{Guid.NewGuid():N}@tests.local", password = "StrongPass123!" },
            cancellationToken);
        response.EnsureSuccessStatusCode();
        using var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync(cancellationToken));
        return json.RootElement.GetProperty("accessToken").GetString()!;
    }
}
