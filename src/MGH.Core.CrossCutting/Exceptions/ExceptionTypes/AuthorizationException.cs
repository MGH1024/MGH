using System.Runtime.Serialization;

namespace MGH.Core.CrossCutting.Exceptions.ExceptionTypes;

public class AuthorizationException : Exception
{
    public AuthorizationException() { }

    [Obsolete("Obsolete")]
    protected AuthorizationException(SerializationInfo info, StreamingContext context) : base(info, context) { }

    public AuthorizationException(string message) : base(message) { }

    public AuthorizationException(string message, Exception innerException) : base(message, innerException) { }
}
