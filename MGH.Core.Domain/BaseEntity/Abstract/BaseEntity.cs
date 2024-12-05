namespace MGH.Core.Domain.BaseEntity.Abstract;

public class BaseEntity<TKey> : IEntity<TKey>,IEquatable<TKey>
{
    public TKey Id { get; protected set; }
    public bool Equals(TKey obj)
    {
        if (obj is BaseEntity<TKey> entity)
            return Equals(entity);

        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    private bool Equals(BaseEntity<TKey> other)
    {
        return other != null && Id.Equals(other.Id);
    }
}