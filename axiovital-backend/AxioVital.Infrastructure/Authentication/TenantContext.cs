using AxioVital.Domain.Interfaces;

namespace AxioVital.Infrastructure.Authentication;

/// <summary>
/// Request-scoped tenant context resolved from JWT claims via middleware.
/// </summary>
public class TenantContext : ITenantProvider
{
    private Guid _tenantId;
    private bool _hasTenant;

    public Guid TenantId => _hasTenant ? _tenantId : throw new InvalidOperationException("Tenant context has not been established.");

    public bool HasTenant => _hasTenant;

    /// <summary>
    /// Sets the tenant context for the current request. Called by tenant resolution middleware.
    /// </summary>
    public void SetTenant(Guid tenantId)
    {
        _tenantId = tenantId;
        _hasTenant = true;
    }
}
