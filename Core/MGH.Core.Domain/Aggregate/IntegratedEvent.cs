namespace MGH.Core.Domain.Aggregate;

public abstract class IntegratedEvent : IEvent
{
    public Guid EventId { get; private set; } = Guid.NewGuid();
}