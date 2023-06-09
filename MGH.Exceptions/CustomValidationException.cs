using System.Net;
using MGH.Exceptions.Base;
using MGH.Exceptions.Models;
using Microsoft.Extensions.Logging;

namespace MGH.Exceptions;

public class CustomValidationException : GeneralException
{
    private const int ExceptionCode = 102;
    public IEnumerable<ValidationError> ValidationErrors { get; }
    
    public CustomValidationException(IEnumerable<ValidationError> validationErrors):
        base(message:"validation exception","",null, HttpStatusCode.BadRequest)
    {
        ErrorCode = ExceptionCode;
        Level = LogLevel.Error;
        ValidationErrors = validationErrors;
    }
}