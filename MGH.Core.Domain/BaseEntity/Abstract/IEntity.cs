namespace MGH.Core.Domain.BaseEntity.Abstract;

public interface IEntity<out T> 
{
    T Id { get; }
}