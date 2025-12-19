namespace MGH.Core.Domain.Events;

public interface IDomainEvent
{
    IEnumerable<DomainEvent> GetDomainEvents();
    void ClearDomainEvents();
}
