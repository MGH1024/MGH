namespace MGH.Exceptions;

public class DuplicateException : GeneralException
{
    public const int ExceptionCode = 100;

    public DuplicateException() : this(null, null)
    {
        ErrorCode = ExceptionCode;
    }

    public DuplicateException(Type entityType) : this(entityType, null)
    {
        ErrorCode = ExceptionCode;
    }

    public DuplicateException(Type entityType, Exception innerException) : base(
        "Title is duplicate!",
        $"Entity type: {entityType.FullName}", innerException)
    {
        EntityType = entityType;
        ErrorCode = ExceptionCode;
    }

    public Type EntityType { get; set; }
}