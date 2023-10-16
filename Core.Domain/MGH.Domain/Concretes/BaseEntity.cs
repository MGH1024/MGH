using MGH.Domain.Abstracts;

namespace MGH.Domain.Concretes;

public class BaseEntity<T> : IEntity<T>
{
    public T Id { get; set; }
}