using MGH.Core.Domain.Events;

namespace MGH.Core.Domain.Base;

public interface IAggregateRoot : IVersion, IEntity
{
    IReadOnlyList<DomainEvent> DomainEvents { get; }
    IReadOnlyList<DomainEvent> IntegratedEvents { get; }
}

public interface IAggregateRoot<T> : IAggregateRoot, IEntity<T>
{
}
