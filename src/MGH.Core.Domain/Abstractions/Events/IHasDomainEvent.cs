using MGH.Core.Domain.Events;

namespace MGH.Core.Domain.Abstractions.Events;

public interface IHasDomainEvent<T>
{
    IEnumerable<DomainEvent<T>> GetDomainEvents();
    void ClearDomainEvents();
}

