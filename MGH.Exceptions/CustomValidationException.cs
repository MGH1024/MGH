using System.Net;
using System.Collections;
using MGH.Exceptions.Models;
using Microsoft.Extensions.Logging;

namespace MGH.Exceptions;

public class CustomValidationException : GeneralException
{
    private const int ExceptionCode = 1;
    public IEnumerable ValidationErrors { get; protected set; }
    
    public CustomValidationException(string message,IEnumerable<ValidationError> validationErrors):
        base(message:message,technicalMessage:"",innerException:null,statusCode: HttpStatusCode.BadRequest)
    {
        ValidationErrors = validationErrors;
        ErrorCode = ExceptionCode;
        Level = LogLevel.Error;
    }
}