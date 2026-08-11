using AxioVital.Infrastructure.Authentication;

namespace AxioVital.Api.Middleware;

/// <summary>
/// Resolves the tenant context from the authenticated user's JWT claims.
/// Must run after authentication middleware.
/// </summary>
public class TenantResolutionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<TenantResolutionMiddleware> _logger;

    public TenantResolutionMiddleware(RequestDelegate next, ILogger<TenantResolutionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, TenantContext tenantContext)
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var tenantClaim = context.User.FindFirst("tenant_id");
            if (tenantClaim != null && Guid.TryParse(tenantClaim.Value, out var tenantId))
            {
                tenantContext.SetTenant(tenantId);
                _logger.LogDebug("Tenant context set to {TenantId}", tenantId);
            }
            else
            {
                _logger.LogWarning("Authenticated user has no valid tenant_id claim");
            }
        }

        await _next(context);
    }
}
