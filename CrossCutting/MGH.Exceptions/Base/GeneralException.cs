﻿using System.Net;
using MGH.Exceptions.Models;
using Microsoft.Extensions.Logging;

namespace MGH.Exceptions.Base;

public class GeneralException : Exception
{
    /// <summary>
    ///     An arbitrary error code.
    /// </summary>
    public int? ErrorCode { get; protected set; }

    /// <summary>
    ///     Technical-details are not allowed to be shown to the user.
    ///     Just log them or use them internally by software-technicians.
    /// </summary>
    public string TechnicalMessage { get; protected set; }

    public HttpStatusCode StatusCode { get; }

    /// <summary>
    ///     Severity of the exception. The main usage will be for logs and monitoring.
    ///     This way we can distinguish various exceptions in logs,
    ///     Think about the difference of between severity of a ValidationException and an Exception related to DB connection
    ///     or Infrastructure.
    ///     Default: Error.
    /// </summary>
    public LogLevel Level { get; protected set; }
    public IEnumerable<ValidationError> ValidationErrors { get; }

    protected GeneralException(string message,int errorCode,HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
        : base(message)
    {
        ErrorCode = errorCode;
        TechnicalMessage = "";
        Level = LogLevel.Error;
        StatusCode = statusCode;
    }

    protected GeneralException(string message, string technicalMessage, Exception innerException,
        HttpStatusCode statusCode = HttpStatusCode.InternalServerError,
        int? errorCode = null)
        : base(message, innerException)
    {
        ErrorCode = errorCode;
        StatusCode = statusCode;
        TechnicalMessage = technicalMessage;
        Level = LogLevel.Error;
    }
    
    protected GeneralException(string message,IEnumerable<ValidationError> validationErrors, string technicalMessage, Exception innerException,
        HttpStatusCode statusCode = HttpStatusCode.InternalServerError,
        int? errorCode = null)
        : base(message, innerException)
    {
        ErrorCode = errorCode;
        StatusCode = statusCode;
        TechnicalMessage = technicalMessage;
        Level = LogLevel.Error;
        ValidationErrors = validationErrors;
    }


    public override string ToString()
    {
        var baseMessage = base.ToString();
        if (!string.IsNullOrEmpty(Message))
        {
            baseMessage = baseMessage.Replace(Message, $"{Message}, TechnicalMessage: {TechnicalMessage}");
        }
        else
        {
            if (InnerException != null)
            {
                var index = baseMessage.IndexOf("--->", StringComparison.Ordinal);
                if (index >= 0)
                    baseMessage = baseMessage.Insert(index, $"TechnicalMessage: {TechnicalMessage}");
            }
            else
            {
                baseMessage = baseMessage + $" TechnicalMessage: {TechnicalMessage}";
            }
        }

        return baseMessage;
    }
}