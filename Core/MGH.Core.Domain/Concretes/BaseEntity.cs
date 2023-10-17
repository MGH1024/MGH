using MGH.Core.Domain.Abstracts;

namespace MGH.Core.Domain.Concretes;

public class BaseEntity<T> : IEntity<T>
{
    public T Id { get; set; }
}