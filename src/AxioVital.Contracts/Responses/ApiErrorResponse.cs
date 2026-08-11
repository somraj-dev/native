namespace AxioVital.Contracts.Responses;

/// <summary>
/// Standard API error response.
/// </summary>
public sealed class ApiErrorResponse
{
    public string Type { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public int Status { get; set; }
    public string? Detail { get; set; }
    public string? TraceId { get; set; }
    public Dictionary<string, string[]>? Errors { get; set; }
}
