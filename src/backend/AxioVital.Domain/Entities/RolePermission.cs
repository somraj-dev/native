namespace AxioVital.Domain.Entities;

/// <summary>
/// Join entity linking roles to permissions (many-to-many).
/// </summary>
public class RolePermission : BaseEntity
{
    public Guid RoleId { get; set; }
    public Role Role { get; set; } = null!;

    public Guid PermissionId { get; set; }
    public Permission Permission { get; set; } = null!;
}
