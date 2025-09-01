using MGH.Core.Domain.Events;

namespace MGH.Core.Infrastructure.EventBus;

public interface IEventBus
{
    /// <summary>
    /// Publishes a single event message to RabbitMQ
    /// </summary>
    void Publish<T>(T model) where T : IEvent;

    /// <summary>
    /// Publishes a batch of events to RabbitMQ
    /// </summary>
    void Publish<T>(IEnumerable<T> models) where T : IEvent;

    /// <summary>
    /// Consume a specific event type with a provided handler function.
    /// </summary>
    void Consume<T>(Func<T, Task> handler) where T : IEvent;

    /// <summary>
    /// Consume a specific event type using the registered IEventHandler<T> from DI.
    /// </summary>
    void Consume<T>() where T : IEvent;
}