namespace MGH.Domain;

public abstract class Entity<TPrimaryKey> : IEntity<TPrimaryKey>, IOrderAble
{
    protected Entity()
    {
        CreatedDate = DateTime.Now;
    }

    public TPrimaryKey Id { get; protected set; } = default;
    public DateTime CreatedDate { get; protected set; }
    public string CreatedBy { get; protected set; }
    public DateTime? UpdatedDate { get; protected set; }
    public string UpdatedBy { get; protected set; }
    public DateTime? DeletedDate { get; protected set; }
    public string DeletedBy { get; protected set; }
    public int Order { get; private set; }
}