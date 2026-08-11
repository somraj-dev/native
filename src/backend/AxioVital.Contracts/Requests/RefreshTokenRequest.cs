namespace AxioVital.Contracts.Requests;

/// <summary>
/// Request to refresh an expired access token.
/// </summary>
public sealed class RefreshTokenRequest
{
    public string RefreshToken { get; set; } = string.Empty;
}
