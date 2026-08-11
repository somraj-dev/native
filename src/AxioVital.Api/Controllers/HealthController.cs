using Microsoft.AspNetCore.Mvc;

namespace AxioVital.Api.Controllers;

/// <summary>
/// Health check controller providing detailed dependency status.
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public class HealthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<HealthController> _logger;

    public HealthController(IConfiguration configuration, ILogger<HealthController> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    /// <summary>
    /// Returns application health status and version information.
    /// </summary>
    [HttpGet("/health/info")]
    public IActionResult GetHealthInfo()
    {
        var response = new Contracts.Responses.HealthResponse
        {
            Status = "Healthy",
            TimestampUtc = DateTime.UtcNow,
            Version = typeof(HealthController).Assembly.GetName().Version?.ToString() ?? "1.0.0",
            Dependencies = new Dictionary<string, string>
            {
                ["postgresql"] = !string.IsNullOrEmpty(_configuration.GetConnectionString("DefaultConnection")) ? "Configured" : "Not Configured",
                ["redis"] = !string.IsNullOrEmpty(_configuration.GetConnectionString("Redis")) ? "Configured" : "Not Configured",
                ["kafka"] = !string.IsNullOrEmpty(_configuration["Kafka:BootstrapServers"]) ? "Configured" : "Not Configured",
                ["minio"] = !string.IsNullOrEmpty(_configuration["Minio:Endpoint"]) ? "Configured" : "Not Configured"
            }
        };

        _logger.LogDebug("Health check requested");
        return Ok(response);
    }
}
