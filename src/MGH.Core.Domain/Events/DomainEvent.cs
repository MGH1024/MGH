using MGH.Core.Domain.Abstractions.Events;

namespace MGH.Core.Domain.Events;

public class DomainEvent<TAggregateId> : IDomainEvent
{
    protected DomainEvent()
    {
        Id = Guid.NewGuid();
        OccurredOn = DateTime.UtcNow;
    }

    protected DomainEvent(TAggregateId aggregateId)
    {
        Id = Guid.NewGuid();
        OccurredOn = DateTime.UtcNow;
        AggregateId = aggregateId;
    }


    public Guid Id { get; }
    public long EventOrder { get; }
    public DateTime OccurredOn { get; }
    public TAggregateId AggregateId { get; protected set; }
}
