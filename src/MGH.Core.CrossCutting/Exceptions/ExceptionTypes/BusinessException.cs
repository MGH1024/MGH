using System.Runtime.Serialization;

namespace MGH.Core.CrossCutting.Exceptions.ExceptionTypes;

public class BusinessException : Exception
{
    public BusinessException() { }

    [Obsolete("Obsolete")]
    protected BusinessException(SerializationInfo info, StreamingContext context) : base(info, context) { }

    public BusinessException(string message) : base(message) { }

    public BusinessException(string message, Exception innerException) : base(message, innerException) { }
}
