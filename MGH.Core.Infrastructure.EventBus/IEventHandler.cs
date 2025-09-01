using MGH.Core.Domain.Events;

namespace MGH.Core.Infrastructure.EventBus;

public interface IEventHandler<in T> where T : IEvent
{
    Task HandleAsync(T message);
}