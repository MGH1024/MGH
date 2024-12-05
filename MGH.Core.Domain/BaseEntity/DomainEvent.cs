using MGH.Core.Domain.BaseEntity.Abstract;

namespace MGH.Core.Domain.BaseEntity;

public class DomainEvent : IEvent
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public DateTime OccurredOn { get; private set; } = DateTime.UtcNow;
}