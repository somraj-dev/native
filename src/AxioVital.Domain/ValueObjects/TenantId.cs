namespace AxioVital.Domain.ValueObjects;

/// <summary>
/// Strongly-typed tenant identifier to prevent accidental misuse of raw GUIDs.
/// </summary>
public readonly record struct TenantId(Guid Value)
{
    public static TenantId New() => new(Guid.NewGuid());
    public static TenantId From(Guid value) => new(value);
    public static TenantId Empty => new(Guid.Empty);

    public override string ToString() => Value.ToString();

    public static implicit operator Guid(TenantId tenantId) => tenantId.Value;
    public static explicit operator TenantId(Guid value) => new(value);
}
