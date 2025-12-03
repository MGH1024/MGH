namespace MGH.Core.Domain.Events;

public class IntegrationEvent : IEvent
{
    private static long _currentOrder = 0;
    public object EventData { get; }
    public Guid Id { get; }
    public DateTime OccurredOn { get; }

    public IntegrationEvent(object eventData)
    {
        EventData = eventData;
        Id = Guid.NewGuid();
        OccurredOn = DateTime.UtcNow;
    }

    public override string ToString()
    {
        return $"{nameof(DomainEvent)} [Id={Id}, Time={OccurredOn:O}, Data={EventData}]";
    }
}