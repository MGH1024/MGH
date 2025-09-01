using MGH.Core.Domain.Events;

namespace MGH.Core.Domain.BaseModels;

public interface IAggregate : IVersion, IEntity
{
    IReadOnlyList<DomainEvent> DomainEvents { get; }
    IEvent[] ClearDomainEvents();
}

public interface IAggregate<T> : IAggregate, IVersion, IEntity<T>
{
}
