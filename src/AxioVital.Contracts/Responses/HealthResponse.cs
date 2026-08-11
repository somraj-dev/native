namespace AxioVital.Contracts.Responses;

/// <summary>
/// Standard health check response.
/// </summary>
public sealed class HealthResponse
{
    public string Status { get; set; } = string.Empty;
    public DateTime TimestampUtc { get; set; }
    public string Version { get; set; } = string.Empty;
    public Dictionary<string, string> Dependencies { get; set; } = new();
}
