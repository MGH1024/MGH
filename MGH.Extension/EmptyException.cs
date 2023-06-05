using System.Runtime.Serialization;

namespace MGH.Extension;

public class EmptyException : Exception
{
    public EmptyException()
    {
    }

    public EmptyException(string message)
        : base(message)
    {
    }

    public EmptyException(string message, Exception inner)
        : base(message, inner)
    {
    }

    protected EmptyException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}