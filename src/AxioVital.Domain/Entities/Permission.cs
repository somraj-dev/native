namespace AxioVital.Domain.Entities;

/// <summary>
/// Represents a permission that can be assigned to roles.
/// </summary>
public class Permission : BaseEntity
{
    /// <summary>
    /// Unique name of the permission (e.g., "patients.read", "appointments.create").
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Human-readable description of what this permission grants.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Category grouping for UI display (e.g., "Patients", "Appointments").
    /// </summary>
    public string? Category { get; set; }

    /// <summary>
    /// Roles that have this permission.
    /// </summary>
    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}
