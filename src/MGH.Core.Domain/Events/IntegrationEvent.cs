using MGH.Core.Domain.Abstractions.Events;

namespace MGH.Core.Domain.Events;

public class IntegrationEvent : IIntegrationEvent
{
    protected IntegrationEvent()
    {
        Id = Guid.NewGuid();
        OccurredOn = DateTime.UtcNow;
    }

    public Guid Id { get; }
    public DateTime OccurredOn { get; }
    public string EventType => GetType().Name;
}