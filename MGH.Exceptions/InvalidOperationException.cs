using System.Net;

namespace MGH.Exceptions;

public class InvalidOperationException : GeneralException
{
    public const int ExceptionCode = 5;

    public InvalidOperationException(string message, string operation = "") :
        base(message,HttpStatusCode.BadRequest)
    {
        Operation = operation;
        ErrorCode = ExceptionCode;
    }

    public InvalidOperationException(string message, string technicalMessage, string operation,
        Exception innerException) : base(message, technicalMessage, innerException,HttpStatusCode.BadRequest ,ExceptionCode)
    {
        Operation = operation;
    }

    public string Operation { get; }
}