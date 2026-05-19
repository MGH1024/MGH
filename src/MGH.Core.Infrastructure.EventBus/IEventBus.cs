using MGH.Core.Domain.Abstractions.Events;
using MGH.Core.Domain.Events;

namespace MGH.Core.Infrastructure.EventBus;

public interface IEventBus
{
    /// <summary>
    /// Publishes a single event message to RabbitMQ
    /// </summary>
    Task PublishAsync<T>(
        T model,
        PublishMode mode,
        CancellationToken cancellationToken) where T : IEventMetadata;

    /// <summary>
    /// Publishes a batch of events to RabbitMQ
    /// </summary>
    Task PublishAsync<T>(
        PublishMode mode,
        IEnumerable<T> models,
        CancellationToken cancellationToken) where T : IEventMetadata;

    /// <summary>
    /// Consume a specific event type with a provided handler function.
    /// </summary>
    Task ConsumeAsync<T>(Func<T, Task> handler) where T : IEventMetadata;

    /// <summary T="from DI.">
    /// Consume a specific event type using the registered IEventHandler
    /// </summary>
    Task ConsumeAsync<T>() where T : IEventMetadata;
}