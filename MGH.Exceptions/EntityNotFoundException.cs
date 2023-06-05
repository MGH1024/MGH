namespace MGH.Exceptions;

public class EntityNotFoundException : GeneralException
{
    public const int ExceptionCode = 2;

    public EntityNotFoundException(Type entityType, object id) : this(entityType, id, null, null)
    {
        ErrorCode = ExceptionCode;
    }

    public EntityNotFoundException(Type entityType) : this(entityType, 0, null, null)
    {
        ErrorCode = ExceptionCode;
    }

    /// <summary>
    ///     Creates a new <see cref="EntityNotFoundException" /> object.
    /// </summary>
    public EntityNotFoundException(Type entityType, object id, Exception innerException,
        IDictionary<string, object> extra)
        : base("Entity not found",
            $"There is no such an entity. Entity type: {entityType.FullName}, id: {id}, extra: {ToFormattedString(extra)}",
            innerException)
    {
        EntityType = entityType;
        Id = id;
        ErrorCode = ExceptionCode;
    }

    /// <summary>
    ///     Type of the entity.
    /// </summary>
    public Type EntityType { get; set; }

    /// <summary>
    ///     Id of the Entity.
    /// </summary>
    public object Id { get; set; }

    private static string ToFormattedString<TKey, TValue>(IDictionary<TKey, TValue> dict, string format = null,
        string separator = null)
    {
        if (dict == null)
            return "";
        separator = !string.IsNullOrEmpty(separator) ? separator : " ";
        format = !string.IsNullOrEmpty(format) ? format : "{0}=\'{1}\'";
        return string.Join(separator, dict.Select(p => string.Format(format, p.Key, p.Value)));
    }
}