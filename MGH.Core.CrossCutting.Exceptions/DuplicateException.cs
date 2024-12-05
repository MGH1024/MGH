using System.Net;
using MGH.Exceptions.Base;
using Microsoft.Extensions.Logging;

namespace MGH.Exceptions;

public class DuplicateException : GeneralException
{
    private const int ExceptionCode = 103;
    public Type EntityType { get; set; }

    public DuplicateException(string message) : base(message, ExceptionCode, HttpStatusCode.Conflict)
    {
        Level = LogLevel.Warning;
    }

    public DuplicateException(string field, Type entityType) : base(
        $"{field} is duplicate in Entity type: {entityType.FullName} ",
        ExceptionCode,
        HttpStatusCode.Conflict)
    {
        EntityType = entityType;
        ErrorCode = ExceptionCode;
    }

    public DuplicateException(string field, Type entityType, Exception innerException) : base(
        $"{field} is duplicate!",
        $"Entity type: {entityType.FullName}",
        innerException,
        HttpStatusCode.Conflict,
        ExceptionCode)
    {
        EntityType = entityType;
        ErrorCode = ExceptionCode;
    }
}