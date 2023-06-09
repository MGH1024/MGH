using System.Net;

namespace MGH.Exceptions;

/// <summary>
///     Indicates the situation in which a request is rejected because the callee believes that the request is already
///     processed and there is no need to process it again.
///     Usually it will happen because more-than-once delivery situations in EventBus.
/// </summary>
public class DuplicateRequestException : GeneralException
{
    public const int ExceptionCode = 3;

    public DuplicateRequestException(object requestData, string message) : base(
        message, HttpStatusCode.Conflict)
    {
        RequestData = requestData;
        ErrorCode = ExceptionCode;
    }

    public DuplicateRequestException(object requestData, string message, string technicalMessage,
        Exception innerException) : base(message, technicalMessage, innerException, HttpStatusCode.Conflict,
        ExceptionCode)
    {
        RequestData = requestData;
    }

    public object RequestData { get; protected set; }
}