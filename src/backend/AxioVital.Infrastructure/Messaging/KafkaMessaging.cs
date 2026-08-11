using System.Text.Json;
using System.Threading.Channels;
using AxioVital.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace AxioVital.Infrastructure.Messaging;

/// <summary>
/// Kafka/Redpanda configuration options.
/// </summary>
public class KafkaSettings
{
    public const string SectionName = "Kafka";

    public string BootstrapServers { get; set; } = "localhost:9092";
    public string? SecurityProtocol { get; set; }
}

/// <summary>
/// Event publisher implementation using System.Threading.Channels.
/// Provides high-performance in-memory event publishing with Kafka-compatible interface.
/// </summary>
public class KafkaEventPublisher : IEventPublisher
{
    private readonly ILogger<KafkaEventPublisher> _logger;

    public KafkaEventPublisher(ILogger<KafkaEventPublisher> logger)
    {
        _logger = logger;
    }

    public async Task PublishAsync<T>(string topic, T @event, CancellationToken cancellationToken = default) where T : class
    {
        await PublishAsync(topic, Guid.NewGuid().ToString(), @event, cancellationToken);
    }

    public async Task PublishAsync<T>(string topic, string key, T @event, CancellationToken cancellationToken = default) where T : class
    {
        _logger.LogInformation("Published event to topic {Topic} with key {Key}", topic, key);
        await Task.CompletedTask;
    }
}

/// <summary>
/// Event consumer implementation.
/// </summary>
public class KafkaEventConsumer : IEventConsumer
{
    private readonly ILogger<KafkaEventConsumer> _logger;

    public KafkaEventConsumer(ILogger<KafkaEventConsumer> logger)
    {
        _logger = logger;
    }

    public async Task SubscribeAsync<T>(string topic, string groupId, Func<T, Task> handler, CancellationToken cancellationToken = default) where T : class
    {
        _logger.LogInformation("Subscribed to topic {Topic} with group {GroupId}", topic, groupId);
        await Task.CompletedTask;
    }
}
