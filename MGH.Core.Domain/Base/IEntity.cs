namespace MGH.Core.Domain.Base;

public interface IEntity<T> : IEntity
{
    public T Id { get; set; }
}

public interface IEntity
{
}