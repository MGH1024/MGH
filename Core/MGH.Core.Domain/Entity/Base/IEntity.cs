namespace MGH.Core.Domain.Entity.Base;

public interface IEntity<out T> 
{
    T Id { get; }
}