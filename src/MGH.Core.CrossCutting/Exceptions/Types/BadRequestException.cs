using System.Runtime.Serialization;

namespace MGH.Core.CrossCutting.Exceptions.Types;

public class BadRequestException : Exception
{
    public BadRequestException() { }

    [Obsolete("Obsolete")]
    protected BadRequestException(SerializationInfo info, StreamingContext context) : base(info, context) { }

    public BadRequestException(string message) : base(message) { }

    public BadRequestException(string message, Exception innerException) : base(message, innerException) { }
}
