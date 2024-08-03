namespace MGH.Core.Domain.Aggregate;

public class DomainEvent : IEvent
{
    public Guid Id { get; private set; } = Guid.NewGuid();
}