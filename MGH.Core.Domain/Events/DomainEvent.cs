namespace MGH.Core.Domain.Events;

public class DomainEvent : IEvent
{
    public Guid Id { get;  set; } = Guid.NewGuid();
    public DateTime OccurredOn { get;  set; } = DateTime.UtcNow;
}