namespace AxioVital.Domain.Enums;

/// <summary>
/// Permission types for fine-grained access control.
/// </summary>
public enum PermissionType
{
    Read = 0,
    Create = 1,
    Update = 2,
    Delete = 3,
    Export = 4,
    Import = 5,
    Approve = 6,
    Manage = 7
}
