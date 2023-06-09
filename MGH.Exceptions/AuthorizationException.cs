﻿using System.Net;

namespace MGH.Exceptions;

/// <summary>
///     Authorization Exception
///     TODO: Describe 401 and 403 usages (exception is used for both situation)
/// </summary>
public class AuthorizationException : GeneralException
{
    public const int ExceptionCode = 6;

    public AuthorizationException(string message) : base(message,
        HttpStatusCode.Unauthorized, ExceptionCode)
    {
        
    }

    public AuthorizationException(string message, string technicalMessage, Exception innerException) : base(
        message, technicalMessage, innerException,HttpStatusCode.Unauthorized, ExceptionCode)
    {
    }
}