using System.Net;
using MGH.Exceptions.Base;

namespace MGH.Exceptions;

/// <summary>
///     Authorization Exception
///     TODO: Describe 401 and 403 usages (exception is used for both situation)
/// </summary>
public class AuthorizationException : GeneralException
{
    private const int ExceptionCode = 100;

    public AuthorizationException(string message) : base(message,ExceptionCode, HttpStatusCode.Unauthorized)
    {
    }

    public AuthorizationException(string message, string technicalMessage, Exception innerException) : 
        base(message, technicalMessage, innerException,HttpStatusCode.Unauthorized, ExceptionCode)
    {
    }
}