namespace MGH.Domain;

public abstract class Entity<TPrimaryKey> : IEntity<TPrimaryKey>
{
    protected Entity()
    {
        CreatedDate = DateTime.Now;
    }

    public TPrimaryKey Id { get; } = default;
    public DateTime CreatedDate { get;  }
    public string CreatedBy { get;  }
    public DateTime? UpdatedDate { get;  }
    public string UpdatedBy { get; }
    public DateTime? DeletedDate { get;  }
    public string DeletedBy { get;  }
}