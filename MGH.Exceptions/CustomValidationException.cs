using System.Net;
using System.Collections;
using MGH.Exceptions.Models;
using Microsoft.Extensions.Logging;

namespace MGH.Exceptions;

public class CustomValidationException : GeneralException
{
    private const int ExceptionCode = 1;
    
    public CustomValidationException(string message,IEnumerable<ValidationError> validationErrors):
        base(message,validationErrors,"",null, HttpStatusCode.BadRequest)
    {
        ErrorCode = ExceptionCode;
        Level = LogLevel.Error;
    }
}