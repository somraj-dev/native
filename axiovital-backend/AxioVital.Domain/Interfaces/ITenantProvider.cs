namespace AxioVital.Domain.Interfaces;

/// <summary>
/// Provides the current tenant context for the request.
/// All tenant-aware operations should resolve the tenant through this interface.
/// </summary>
public interface ITenantProvider
{
    /// <summary>
    /// Gets the current tenant ID for the request.
    /// </summary>
    Guid TenantId { get; }

    /// <summary>
    /// Whether a valid tenant context has been established.
    /// </summary>
    bool HasTenant { get; }
}
