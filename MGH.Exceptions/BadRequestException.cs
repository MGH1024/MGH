using System.Net;
using MGH.Exceptions.Base;
using Microsoft.Extensions.Logging;

namespace MGH.Exceptions;

public class BadRequestException : GeneralException
{
    private const int ExceptionCode = 101;
    
    public BadRequestException(string message) : base(message,ExceptionCode, HttpStatusCode.BadRequest)
    {
        Level = LogLevel.Warning;
    }

    public BadRequestException(string message,string technicalMessage, Exception innerException = null) :
        base(message, technicalMessage, innerException, HttpStatusCode.BadRequest,ExceptionCode)
    {
        Level = LogLevel.Warning;
    }
}