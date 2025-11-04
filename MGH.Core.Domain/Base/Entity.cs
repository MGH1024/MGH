namespace MGH.Core.Domain.Base;

public abstract class Entity<T> : IEntity<T>
{
    public T Id { get; set; }
}