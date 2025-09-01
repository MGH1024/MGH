namespace MGH.Core.Domain.BaseModels;

public interface IEntity<T> : IEntity
{
    public T Id { get; set; }
}

public interface IEntity
{
    DateTime CreatedAt { get; set; }

    string CreatedBy { get; set; }
    string CreatedByIp { get; set; }

    DateTime? UpdatedAt { get; set; }

    string UpdatedBy { get; set; }
    string UpdatedByIp { get; set; }

    DateTime? DeletedAt { get; set; }

    string DeletedBy { get; set; }
    string DeletedByIp { get; set; }
}