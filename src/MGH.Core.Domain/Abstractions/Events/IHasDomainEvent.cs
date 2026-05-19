using MGH.Core.Domain.Events;

namespace MGH.Core.Domain.Abstractions.Events;

public interface IHasDomainEvent
{
    IEnumerable<DomainEvent> GetDomainEvents();
    void ClearDomainEvents();
}
