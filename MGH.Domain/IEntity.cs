namespace MGH.Domain;

public interface IEntity<out TPrimaryKey> : IEntity
{
    TPrimaryKey Id { get; }
}

public interface IEntity
{
    DateTime CreatedAt { get; }

    string CreatedBy { get; }

    DateTime? UpdatedAt { get; }

    string UpdatedBy { get; }

    DateTime? DeletedAt { get; }

    string DeletedBy { get; }
}