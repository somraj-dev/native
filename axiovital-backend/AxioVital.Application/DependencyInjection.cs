using AxioVital.Application.Interfaces;
using AxioVital.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AxioVital.Application;

/// <summary>
/// Registers application layer services into the DI container.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Document management
        services.AddScoped<IDocumentService, DocumentService>();

        return services;
    }
}
