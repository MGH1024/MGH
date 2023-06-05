using System.ComponentModel.DataAnnotations;
using System.Net;
using Microsoft.Extensions.Logging;

namespace MGH.Exceptions;


public class BadRequestException : GeneralException
{
    public const int ExceptionCode = 120;

    
    public BadRequestException(string message) : base(message, "",
        HttpStatusCode.BadRequest)
    {
        ErrorCode = ExceptionCode;
        Level = LogLevel.Warning;
    }

   
    public BadRequestException(string message, Exception innerException = null) : base(message, "",
        innerException, HttpStatusCode.BadRequest)
    {
        ValidationErrors =  new List<ValidationResult>(){new(message)};
        ErrorCode = ExceptionCode;
        Level = LogLevel.Warning;
    }

   
    public IList<ValidationResult> ValidationErrors { get; protected set; }
}