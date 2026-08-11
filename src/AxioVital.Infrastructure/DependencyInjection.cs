using Minio;
using AxioVital.Application.Interfaces;
using AxioVital.Domain.Interfaces;
using AxioVital.Infrastructure.Authentication;
using AxioVital.Infrastructure.Caching;
using AxioVital.Infrastructure.Messaging;
using AxioVital.Infrastructure.Persistence;
using AxioVital.Infrastructure.Repositories;
using AxioVital.Infrastructure.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace AxioVital.Infrastructure;

/// <summary>
/// Registers infrastructure services into the DI container.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // PostgreSQL + EF Core
        services.AddDbContext<AxioVitalDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                npgsql =>
                {
                    npgsql.MigrationsAssembly(typeof(AxioVitalDbContext).Assembly.FullName);
                    npgsql.EnableRetryOnFailure(3);
                }
            ));

        // Unit of Work
        services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<AxioVitalDbContext>());

        // Repositories
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        // Tenant Context (request-scoped)
        services.AddScoped<TenantContext>();
        services.AddScoped<ITenantProvider>(provider => provider.GetRequiredService<TenantContext>());

        // Authentication
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
        services.AddSingleton<IPasswordHasher, Argon2PasswordHasher>();
        services.AddSingleton<ITokenService, JwtTokenService>();

        // Redis
        var redisConnection = configuration.GetConnectionString("Redis");
        if (!string.IsNullOrEmpty(redisConnection))
        {
            services.AddSingleton<IConnectionMultiplexer>(_ =>
                ConnectionMultiplexer.Connect(redisConnection));
            services.AddSingleton<ICacheService, RedisCacheService>();
        }

        // Kafka / Messaging
        services.AddSingleton<IEventPublisher, KafkaEventPublisher>();
        services.AddSingleton<IEventConsumer, KafkaEventConsumer>();

        // MinIO
        var minioSettings = configuration.GetSection(MinioSettings.SectionName).Get<MinioSettings>();
        if (minioSettings != null && !string.IsNullOrEmpty(minioSettings.Endpoint))
        {
            services.AddSingleton<IMinioClient>(_ =>
            {
                return new MinioClient()
                    .WithEndpoint(minioSettings.Endpoint)
                    .WithCredentials(minioSettings.AccessKey, minioSettings.SecretKey)
                    .WithSSL(minioSettings.Secure)
                    .Build();
            });
            services.AddSingleton<IObjectStorageService, MinioStorageService>();
        }

        return services;
    }
}
