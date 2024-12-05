using System.Net;
using MGH.Exceptions.Base;

namespace MGH.Exceptions;

public class InvalidOperationException : GeneralException
{
    private const int ExceptionCode = 106;
    public string Operation { get; }

    public InvalidOperationException(string message, string operation = "") :
        base(message,ExceptionCode,HttpStatusCode.BadRequest)
    {
        Operation = operation;
        ErrorCode = ExceptionCode;
    }

    public InvalidOperationException(string message, string technicalMessage, string operation,
        Exception innerException) : base(message, technicalMessage, innerException,HttpStatusCode.BadRequest ,ExceptionCode)
    {
        Operation = operation;
    }
}