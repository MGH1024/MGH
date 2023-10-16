namespace MGH.Domain.Abstracts;

public interface IEntity<out T> 
{
    T Id { get; }
}