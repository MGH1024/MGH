using MGH.Core.Domain.Abstractions;

namespace MGH.Core.Domain.Entities;

public abstract class Entity<T> : IEntity<T>
{
    public required T Id { get; set; }
}