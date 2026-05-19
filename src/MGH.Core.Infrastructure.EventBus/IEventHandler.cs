using MGH.Core.Domain.Abstractions.Events;
using MGH.Core.Domain.Events;

namespace MGH.Core.Infrastructure.EventBus;

public interface IEventHandler<in T> where T : IEventMetadata
{
    Task HandleAsync(T message);
}