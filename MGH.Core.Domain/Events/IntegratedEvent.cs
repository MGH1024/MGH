namespace MGH.Core.Domain.Events;

public abstract class IntegratedEvent : IEvent
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime OccurredOn { get; set; } = DateTime.UtcNow;
}