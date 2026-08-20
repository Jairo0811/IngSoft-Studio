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
        var registerBody = await registerResponse.Content.ReadAsStringAsync(cancellationToken);
        Assert.True(
            registerResponse.StatusCode == HttpStatusCode.OK,
            $"Registration failed with {registerResponse.StatusCode}: {registerBody}");

        using var registerJson = JsonDocument.Parse(registerBody);
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

    [Fact]
    public async Task UserCannotReadAnotherUsersProject()
    {
        using var ownerClient = factory.CreateClient();
        using var otherClient = factory.CreateClient();
        var cancellationToken = TestContext.Current.CancellationToken;

        await RegisterAndAuthenticateAsync(ownerClient, cancellationToken);
        var projectResponse = await ownerClient.PostAsJsonAsync(
            "/api/v1/projects",
            new { name = "Proyecto privado", description = "Solo pertenece a su creador" },
            cancellationToken);
        Assert.Equal(HttpStatusCode.Created, projectResponse.StatusCode);

        using var projectJson = JsonDocument.Parse(
            await projectResponse.Content.ReadAsStringAsync(cancellationToken));
        var projectId = projectJson.RootElement.GetProperty("id").GetGuid();

        await RegisterAndAuthenticateAsync(otherClient, cancellationToken);

        var projectRead = await otherClient.GetAsync(
            $"/api/v1/projects/{projectId}", cancellationToken);
        var requirementsRead = await otherClient.GetAsync(
            $"/api/v1/projects/{projectId}/requirements", cancellationToken);
        var qualityRead = await otherClient.GetAsync(
            $"/api/v1/projects/{projectId}/quality", cancellationToken);

        Assert.Equal(HttpStatusCode.NotFound, projectRead.StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, requirementsRead.StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, qualityRead.StatusCode);
    }

    [Fact]
    public async Task AnonymousRequestsAndOversizedInputAreRejected()
    {
        using var anonymousClient = factory.CreateClient();
        using var authenticatedClient = factory.CreateClient();
        var cancellationToken = TestContext.Current.CancellationToken;

        var anonymousResponse = await anonymousClient.GetAsync(
            "/api/v1/projects", cancellationToken);
        Assert.Equal(HttpStatusCode.Unauthorized, anonymousResponse.StatusCode);

        await RegisterAndAuthenticateAsync(authenticatedClient, cancellationToken);
        var invalidResponse = await authenticatedClient.PostAsJsonAsync(
            "/api/v1/projects",
            new { name = new string('x', 151), description = "Invalid input" },
            cancellationToken);

        Assert.Equal(HttpStatusCode.BadRequest, invalidResponse.StatusCode);
    }

    [Fact]
    public async Task PasswordChangeRevokesThePreviousToken()
    {
        using var client = factory.CreateClient();
        var cancellationToken = TestContext.Current.CancellationToken;
        await RegisterAndAuthenticateAsync(client, cancellationToken);

        var changeResponse = await client.PostAsJsonAsync(
            "/api/auth/change-password",
            new
            {
                currentPassword = "StrongPass123!",
                newPassword = "DifferentPass456!"
            },
            cancellationToken);

        Assert.Equal(HttpStatusCode.NoContent, changeResponse.StatusCode);

        var oldTokenResponse = await client.GetAsync(
            "/api/auth/me", cancellationToken);
        Assert.Equal(HttpStatusCode.Unauthorized, oldTokenResponse.StatusCode);
    }

    private static async Task RegisterAndAuthenticateAsync(
        HttpClient client,
        CancellationToken cancellationToken)
    {
        var response = await client.PostAsJsonAsync(
            "/api/auth/register",
            new
            {
                fullName = "Isolation Test",
                email = $"isolation-{Guid.NewGuid():N}@tests.local",
                password = "StrongPass123!"
            },
            cancellationToken);

        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
        Assert.True(
            response.StatusCode == HttpStatusCode.OK,
            $"Registration failed with {response.StatusCode}: {responseBody}");
        using var json = JsonDocument.Parse(responseBody);
        var token = json.RootElement.GetProperty("accessToken").GetString();
        Assert.False(string.IsNullOrWhiteSpace(token));
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
    }
}
