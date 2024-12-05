namespace MGH.Core.Domain.BaseEntity.Abstract;

public abstract class IntegratedEvent : IEvent
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public DateTime OccurredOn { get; private set; } = DateTime.UtcNow;
}