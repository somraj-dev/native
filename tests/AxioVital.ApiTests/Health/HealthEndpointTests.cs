using System.Net;
using System.Net.Http.Json;
using AxioVital.Api;
using AxioVital.Contracts.Responses;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace AxioVital.ApiTests.Health;

public class HealthEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public HealthEndpointTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetHealthInfo_ShouldReturn200OK_WithHealthDetails()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/health/info");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<HealthResponse>();
        content.Should().NotBeNull();
        content!.Status.Should().Be("Healthy");
        content.Dependencies.Should().ContainKey("postgresql");
    }
}
