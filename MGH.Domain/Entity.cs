namespace MGH.Domain;

public abstract class Entity<TPrimaryKey> : IEntity<TPrimaryKey>
{
    protected Entity()
    {
        CreatedAt = DateTime.Now;
    }

    public TPrimaryKey Id { get; } = default;
    public DateTime CreatedAt { get;  }
    public string CreatedBy { get;  }
    public DateTime? UpdatedAt { get;  }
    public string UpdatedBy { get; }
    public DateTime? DeletedAt { get;  }
    public string DeletedBy { get;  }
}