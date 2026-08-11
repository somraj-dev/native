namespace AxioVital.Application.Interfaces;

/// <summary>
/// Abstraction for WebAuthn (FIDO2) authentication.
/// Implementations will handle registration and assertion ceremonies.
/// </summary>
public interface IWebAuthnService
{
    /// <summary>
    /// Creates WebAuthn registration options for a user.
    /// </summary>
    Task<object> CreateRegistrationOptionsAsync(Guid userId, string displayName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Verifies a WebAuthn registration response.
    /// </summary>
    Task<bool> VerifyRegistrationAsync(Guid userId, object attestationResponse, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates WebAuthn assertion options for authentication.
    /// </summary>
    Task<object> CreateAssertionOptionsAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Verifies a WebAuthn assertion (login) response.
    /// </summary>
    Task<bool> VerifyAssertionAsync(Guid userId, object assertionResponse, CancellationToken cancellationToken = default);
}
