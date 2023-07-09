namespace MGH.Domain;

public interface IEntity<out TPrimaryKey> : IEntity
{
    TPrimaryKey Id { get; }
}

public interface IEntity
{
    DateTime CreatedDate { get; }

    string CreatedBy { get; }

    DateTime? UpdatedDate { get; }

    string UpdatedBy { get; }

    DateTime? DeletedDate { get; }

    string DeletedBy { get; }
}