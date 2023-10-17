namespace MGH.Core.Domain.Abstracts;

public interface IEntity<out T> 
{
    T Id { get; }
}