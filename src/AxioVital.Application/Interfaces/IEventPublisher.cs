namespace AxioVital.Application.Interfaces;

/// <summary>
/// Abstraction for publishing domain/integration events to a message broker.
/// </summary>
public interface IEventPublisher
{
    /// <summary>
    /// Publishes an event to the specified topic.
    /// </summary>
    Task PublishAsync<T>(string topic, T @event, CancellationToken cancellationToken = default) where T : class;

    /// <summary>
    /// Publishes an event with a specific key for partitioning.
    /// </summary>
    Task PublishAsync<T>(string topic, string key, T @event, CancellationToken cancellationToken = default) where T : class;
}

/// <summary>
/// Abstraction for consuming events from a message broker.
/// </summary>
public interface IEventConsumer
{
    /// <summary>
    /// Subscribes to a topic and processes messages with the provided handler.
    /// </summary>
    Task SubscribeAsync<T>(string topic, string groupId, Func<T, Task> handler, CancellationToken cancellationToken = default) where T : class;
}
