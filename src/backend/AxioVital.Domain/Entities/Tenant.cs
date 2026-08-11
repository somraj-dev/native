namespace AxioVital.Domain.Entities;

/// <summary>
/// Represents a tenant (organization) in the multi-tenant system.
/// </summary>
public class Tenant : AuditableEntity
{
    /// <summary>
    /// Display name of the tenant organization.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Unique identifier/slug for the tenant.
    /// </summary>
    public string Identifier { get; set; } = string.Empty;

    /// <summary>
    /// Whether the tenant is currently active.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Navigation property for users belonging to this tenant.
    /// </summary>
    public ICollection<User> Users { get; set; } = new List<User>();
}
