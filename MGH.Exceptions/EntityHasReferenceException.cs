namespace MGH.Exceptions;

public class EntityHasReferenceException : GeneralException
{
    public const int ExceptionCode = 101;

    public EntityHasReferenceException() : this(entityType: null, innerException: null)
    {
        ErrorCode = ExceptionCode;
    }

    public EntityHasReferenceException(Type entityType) : this(entityType, innerException: null)
    {
        ErrorCode = ExceptionCode;
    }


    public EntityHasReferenceException(Type entityType, Exception innerException) : base(
        "An error occurred while saving the entity changes. See the inner exception for details.",
        $"Entity type: {entityType.FullName}", innerException)
    {
        EntityType = entityType;
        ErrorCode = ExceptionCode;
    }

    public Type EntityType { get; set; }
}