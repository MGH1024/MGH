namespace MGH.Exceptions.Models;

public class ValidationError
{
    public ValidationError(string propName, string message)
    {
        PropName = propName;
        Message = message;
    }
    public string PropName { get; }
    public string Message { get; }
}