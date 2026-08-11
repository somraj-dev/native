namespace AxioVital.Application.Interfaces;

/// <summary>
/// Service for hashing and verifying passwords using Argon2id.
/// </summary>
public interface IPasswordHasher
{
    /// <summary>
    /// Hashes a plaintext password using Argon2id.
    /// </summary>
    string HashPassword(string password);

    /// <summary>
    /// Verifies a plaintext password against a stored hash.
    /// </summary>
    bool VerifyPassword(string password, string hash);
}
