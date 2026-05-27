using MGH.Core.Domain.Events;
using MGH.Core.Domain.Abstractions;
using MGH.Core.Domain.Entities.Base;
using MGH.Core.Domain.Abstractions.Events;

namespace MGH.Core.Domain.Entities;

public abstract class AggregateRoot<T> :
    FullAuditable<T>,
    IAggregateRoot<T>,
    IHasDomainEvent<T>,
    IHasIntegrationEvent<T>
{
    private readonly List<DomainEvent<T>> _domainEvents = new();
    public IReadOnlyList<DomainEvent<T>> DomainEvents
        => _domainEvents.AsReadOnly();

    public virtual IEnumerable<DomainEvent<T>> GetDomainEvents()
    {
        return _domainEvents.ToList();
    }

    protected void AddDomainEvent(DomainEvent<T> domainEvent)
    {
        if (domainEvent is null)
            throw new ArgumentNullException(nameof(domainEvent));

        if (_domainEvents.Contains(domainEvent))
            throw new InvalidOperationException(
                "The same domain event instance cannot be added more than once.");

        _domainEvents.Add(domainEvent);
    }

    public virtual void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    private readonly List<IntegrationEvent<T>> _integrationEvents = new();

    public IReadOnlyList<IntegrationEvent<T>> IntegrationEvent
        => _integrationEvents.AsReadOnly();

    public virtual IEnumerable<IntegrationEvent<T>> GetIntegrationEvents()
    {
        return _integrationEvents.ToList();
    }

    protected void AddIntegrationEvent(IntegrationEvent<T> integrationEvent)
    {
        if (integrationEvent is null)
            throw new ArgumentNullException(nameof(integrationEvent));

        if (_integrationEvents.Contains(integrationEvent))
            throw new InvalidOperationException(
                "The same integration event instance cannot be added more than once.");

        _integrationEvents.Add(integrationEvent);
    }

    public virtual void ClearIntegrationEvents()
    {
        _integrationEvents.Clear();
    }

    public int Version { get; protected set; }

    protected void IncrementVersion() => Version++;
    protected void SetVersion(int version) => Version = version;

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
            return true;

        if (obj is not AggregateRoot<T> other)
            return false;

        if (Id is null || other.Id is null)
            return false;

        return Id.Equals(other.Id);
    }

    public override int GetHashCode()
    {
        return Id?.GetHashCode() ?? 0;
    }

    public static bool operator ==(AggregateRoot<T>? left, AggregateRoot<T>? right)
    {
        if (left is null && right is null)
            return true;
        if (left is null || right is null)
            return false;
        return left.Equals(right);
    }

    public static bool operator !=(AggregateRoot<T>? left, AggregateRoot<T>? right)
        => !(left == right);
}