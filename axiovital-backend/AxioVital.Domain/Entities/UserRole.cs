namespace AxioVital.Domain.Entities;

/// <summary>
/// Join entity linking users to roles (many-to-many).
/// </summary>
public class UserRole : BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public Guid RoleId { get; set; }
    public Role Role { get; set; } = null!;

    /// <summary>
    /// When the role was assigned.
    /// </summary>
    public DateTime AssignedAtUtc { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Who assigned this role.
    /// </summary>
    public Guid? AssignedBy { get; set; }
}
