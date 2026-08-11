namespace AxioVital.Domain.Entities;

/// <summary>
/// Represents a role that can be assigned to users for RBAC.
/// </summary>
public class Role : AuditableEntity
{
    /// <summary>
    /// The tenant this role belongs to. Null for system-wide roles.
    /// </summary>
    public Guid? TenantId { get; set; }

    /// <summary>
    /// Unique name of the role (e.g., "Administrator", "Physician", "Nurse").
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Human-readable description of the role.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Whether this is a system-defined role (cannot be deleted by tenants).
    /// </summary>
    public bool IsSystemRole { get; set; }

    /// <summary>
    /// Navigation property for users assigned this role.
    /// </summary>
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

    /// <summary>
    /// Permissions granted to this role.
    /// </summary>
    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}
