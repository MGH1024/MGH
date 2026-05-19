using MGH.Core.Domain.Events;

namespace MGH.Core.Domain.Abstractions;

public interface IAggregateRoot : IEntity
{
    IReadOnlyList<DomainEvent> DomainEvents { get; }
}

public interface IAggregateRoot<T> : IAggregateRoot, IEntity<T>
{
}
