using MGH.Core.Domain.BaseEntity.Abstract;

namespace MGH.Core.Infrastructure.EventBus;

public interface IEventBusDispatcher
{
    void Publish<T>(T model) where T : IntegratedEvent;
}