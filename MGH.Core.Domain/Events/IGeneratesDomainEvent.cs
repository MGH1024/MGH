namespace MGH.Core.Domain.Events;

public interface IGeneratesDomainEvent
{
    IEnumerable<DomainEvent> GetDomainEvents();

    IEnumerable<DomainEvent> GetIntegratedEvents();

    void ClearDomainEvents();

    void ClearIntegratedEvents();
}
