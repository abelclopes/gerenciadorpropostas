using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Text.Json;
using Xunit;

namespace GerenciadorPropostas.FunctionalTests;

public class ApiAuthenticationTests
{
    private static string BaseUrl =>
        Environment.GetEnvironmentVariable("FUNCTIONAL_TEST_BASE_URL")?.TrimEnd('/')
        ?? "http://localhost:18132";

    [Fact]
    public async Task Login_ComUsuarioSeed_DeveRetornarToken()
    {
        using var http = new HttpClient { BaseAddress = new Uri(BaseUrl), Timeout = TimeSpan.FromSeconds(20) };

        using var response = await http.PostAsJsonAsync("/api/auth", new
        {
            email = "abell@gmail.com",
            password = "teste123"
        });

        response.EnsureSuccessStatusCode();

        var body = await response.Content.ReadAsStringAsync();
        using var json = JsonDocument.Parse(body);

        Assert.True(json.RootElement.TryGetProperty("token", out var tokenNode));
        Assert.False(string.IsNullOrWhiteSpace(tokenNode.GetString()));
    }

    [Fact]
    public async Task LoginEConsultaClans_ComUsuarioSeed_DeveRetornarSucesso()
    {
        using var http = new HttpClient { BaseAddress = new Uri(BaseUrl), Timeout = TimeSpan.FromSeconds(20) };

        using var loginResponse = await http.PostAsJsonAsync("/api/auth", new
        {
            email = "abell@gmail.com",
            password = "teste123"
        });

        loginResponse.EnsureSuccessStatusCode();
        var loginBody = await loginResponse.Content.ReadAsStringAsync();
        using var loginJson = JsonDocument.Parse(loginBody);
        var token = loginJson.RootElement.GetProperty("token").GetString();

        Assert.False(string.IsNullOrWhiteSpace(token));

        http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        http.DefaultRequestHeaders.Add("x-access-token", token);

        using var clansResponse = await http.GetAsync("/api/usuarios/clans/abell@gmail.com");
        clansResponse.EnsureSuccessStatusCode();

        var clansBody = await clansResponse.Content.ReadAsStringAsync();
        using var clansJson = JsonDocument.Parse(clansBody);

        Assert.True(clansJson.RootElement.TryGetProperty("email", out var emailNode));
        Assert.Equal("abell@gmail.com", emailNode.GetString());
    }

    [Fact]
    public async Task LoginEConsultaPermissoes_DeveRetornarSucesso()
    {
        using var http = new HttpClient { BaseAddress = new Uri(BaseUrl), Timeout = TimeSpan.FromSeconds(20) };

        using var loginResponse = await http.PostAsJsonAsync("/api/auth", new
        {
            email = "abell@gmail.com",
            password = "teste123"
        });

        loginResponse.EnsureSuccessStatusCode();
        var loginBody = await loginResponse.Content.ReadAsStringAsync();
        using var loginJson = JsonDocument.Parse(loginBody);
        var token = loginJson.RootElement.GetProperty("token").GetString();

        Assert.False(string.IsNullOrWhiteSpace(token));

        http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        http.DefaultRequestHeaders.Add("x-access-token", token);

        using var permissoesResponse = await http.GetAsync("/api/usuarios/permissoes");
        permissoesResponse.EnsureSuccessStatusCode();

        var body = await permissoesResponse.Content.ReadAsStringAsync();
        using var json = JsonDocument.Parse(body);
        Assert.Equal(JsonValueKind.Array, json.RootElement.ValueKind);
        Assert.True(json.RootElement.GetArrayLength() > 0);
    }
}
