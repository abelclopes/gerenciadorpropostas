using Xunit;

namespace GerenciadorPropostas.FunctionalTests;

public class ApiSwaggerTests
{
    private static string BaseUrl =>
        Environment.GetEnvironmentVariable("FUNCTIONAL_TEST_BASE_URL")?.TrimEnd('/')
        ?? "http://localhost:18132";

    [Fact]
    public async Task SwaggerJson_DeveResponderComSucesso()
    {
        using var http = new HttpClient { BaseAddress = new Uri(BaseUrl), Timeout = TimeSpan.FromSeconds(20) };

        using var response = await http.GetAsync("/swagger/v1/swagger.json");
        response.EnsureSuccessStatusCode();
    }
}
