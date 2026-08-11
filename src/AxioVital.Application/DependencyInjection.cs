using Microsoft.Extensions.DependencyInjection;

namespace AxioVital.Application;

/// <summary>
/// Registers application layer services into the DI container.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Application services will be registered here as they are implemented.
        // Example:
        // services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}
