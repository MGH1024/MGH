using System.Net;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using MGH.Exceptions.Models;

namespace MGH.Exceptions;

public class CustomValidationException : GeneralException
{
    public const int ExceptionCode = 1;
    
    public CustomValidationException(string message, IList<ValidationResult> validationErrors) : base(message, "",
        HttpStatusCode.BadRequest)
    {
        Level = LogLevel.Warning;
        ErrorCode = ExceptionCode;
        ValidationErrors = validationErrors ?? new List<ValidationResult>();
    }

   
    public CustomValidationException(string message, Exception innerException = null) : base(message, "",
        innerException, HttpStatusCode.BadRequest)
    {
        ValidationErrors =  new List<ValidationResult>(){new(message)};
        ErrorCode = ExceptionCode;
        Level = LogLevel.Warning;
    }

    public CustomValidationException(string message,IEnumerable<ValidationError> validationErrors,
        Exception innerException=null):base(message,"",innerException,HttpStatusCode.BadRequest)
    {
        ValidationErrors =  new List<ValidationResult>(){new(message)};
        ErrorCode = ExceptionCode;
        Level = LogLevel.Error;
    }
    
    public IList<ValidationResult> ValidationErrors { get; protected set; }
}