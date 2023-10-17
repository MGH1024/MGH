namespace MGH.Core.Domain.Abstracts;

public interface IAuditable
{
    DateTime CreatedAt { get; }

    string CreatedBy { get; }

    DateTime? UpdatedAt { get; }

    string UpdatedBy { get; }

    DateTime? DeletedAt { get; }

    string DeletedBy { get; }
}