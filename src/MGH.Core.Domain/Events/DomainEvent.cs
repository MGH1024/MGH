using MGH.Core.Domain.Abstractions.Events;

namespace MGH.Core.Domain.Events;

public class DomainEvent : IDomainEvent
{
    protected DomainEvent()
    {
        Id = Guid.NewGuid();
        OccurredOn = DateTime.UtcNow;
    }

    public Guid Id { get; }
    public DateTime OccurredOn { get; }
}
