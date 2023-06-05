using System.Net;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using MGH.Exceptions.Models;

namespace MGH.Exceptions;

public class ValidationException : GeneralException
{
    public const int ExceptionCode = 1;
    
    public ValidationException(string message, IList<ValidationResult> validationErrors) : base(message, "",
        HttpStatusCode.BadRequest)
    {
        Level = LogLevel.Warning;
        ErrorCode = ExceptionCode;
        ValidationErrors = validationErrors ?? new List<ValidationResult>();
    }

   
    public ValidationException(string message, Exception innerException = null) : base(message, "",
        innerException, HttpStatusCode.BadRequest)
    {
        ValidationErrors =  new List<ValidationResult>(){new(message)};
        ErrorCode = ExceptionCode;
        Level = LogLevel.Warning;
    }

    public ValidationException(string message,IEnumerable<ValidationError> validationErrors,
        Exception innerException=null):base(message,"",innerException,HttpStatusCode.BadRequest)
    {
        ValidationErrors =  new List<ValidationResult>(){new(message)};
        ErrorCode = ExceptionCode;
        Level = LogLevel.Error;
    }
    
    public IList<ValidationResult> ValidationErrors { get; protected set; }
}