namespace MGH.Core.Domain.Abstractions;

public interface IAggregateRoot : IEntity
{
}

public interface IAggregateRoot<T> : IAggregateRoot, IEntity<T>
{
}
