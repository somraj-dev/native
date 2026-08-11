namespace AxioVital.Application.Interfaces;

/// <summary>
/// Service for JWT token generation and validation.
/// </summary>
public interface ITokenService
{
    /// <summary>
    /// Generates an access token for the given user claims.
    /// </summary>
    string GenerateAccessToken(Guid userId, Guid tenantId, string email, IEnumerable<string> roles);

    /// <summary>
    /// Generates a refresh token.
    /// </summary>
    string GenerateRefreshToken();

    /// <summary>
    /// Validates an access token and returns the claims principal.
    /// Returns null if the token is invalid.
    /// </summary>
    System.Security.Claims.ClaimsPrincipal? ValidateToken(string token);
}
