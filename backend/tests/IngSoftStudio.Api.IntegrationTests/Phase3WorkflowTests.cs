using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace IngSoftStudio.Api.IntegrationTests;

public sealed class Phase3WorkflowTests(ApiFactory factory) : IClassFixture<ApiFactory>
{
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
}
