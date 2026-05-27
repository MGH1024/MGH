using MGH.Core.Domain.Abstractions.Events;

namespace MGH.Core.Domain.Events;

public class IntegrationEvent<TAggregateId> : IIntegrationEvent
{
    protected IntegrationEvent()
    {
        Id = Guid.NewGuid();
        OccurredOn = DateTime.UtcNow;
    }

    protected IntegrationEvent(TAggregateId aggregateId)
    {
        Id = Guid.NewGuid();
        OccurredOn = DateTime.UtcNow;
        AggregateId = aggregateId;
    }

    public Guid Id { get; }
    public long EventOrder { get; }
    public DateTime OccurredOn { get; }
    public string EventType => GetType().Name;
    public TAggregateId AggregateId { get; protected set; }
}