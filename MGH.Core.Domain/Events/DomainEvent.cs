namespace MGH.Core.Domain.Events;

public class DomainEvent : IEvent
{
    private static long _currentOrder = 0;
    public object EventData { get; }
    public long EventOrder { get; }
    public Guid Id { get; }
    public DateTime OccurredOn { get; }
    public string EventType { get; }

    public DomainEvent(object eventData)
    {
        Id = Guid.NewGuid();
        EventData = eventData;
        EventOrder = GetNextOrder();
        OccurredOn = DateTime.UtcNow;
    }

    public DomainEvent(object eventData, string type)
    {
        EventType = type;
        Id = Guid.NewGuid();
        EventData = eventData;
        EventOrder = GetNextOrder();
        OccurredOn = DateTime.UtcNow;
    }

    private static long GetNextOrder()
    {
        return Interlocked.Increment(ref _currentOrder);
    }

    public override string ToString()
    {
        return $"{nameof(DomainEvent)} [Id={Id}, Order={EventOrder}, Time={OccurredOn:O}, Data={EventData}]";
    }
}
