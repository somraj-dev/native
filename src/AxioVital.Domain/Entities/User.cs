namespace AxioVital.Domain.Entities;

/// <summary>
/// Represents a user in the system. Users belong to a tenant.
/// </summary>
public class User : AuditableEntity
{
    /// <summary>
    /// The tenant this user belongs to.
    /// </summary>
    public Guid TenantId { get; set; }

    /// <summary>
    /// User's email address, used for authentication.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Argon2id hashed password. Never store plaintext.
    /// </summary>
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>
    /// User's first name.
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// User's last name.
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Whether the user account is active.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Last successful login timestamp in UTC.
    /// </summary>
    public DateTime? LastLoginAtUtc { get; set; }

    /// <summary>
    /// Hashed refresh token for JWT refresh flow.
    /// </summary>
    public string? RefreshTokenHash { get; set; }

    /// <summary>
    /// Expiry of the current refresh token.
    /// </summary>
    public DateTime? RefreshTokenExpiryUtc { get; set; }

    /// <summary>
    /// Navigation property to the tenant.
    /// </summary>
    public Tenant Tenant { get; set; } = null!;

    /// <summary>
    /// Roles assigned to this user.
    /// </summary>
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
