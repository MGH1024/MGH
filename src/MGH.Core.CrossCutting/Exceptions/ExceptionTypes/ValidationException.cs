namespace MGH.Core.CrossCutting.Exceptions.ExceptionTypes;

public class ValidationException : Exception
{
    public IEnumerable<ValidationExceptionModel> Errors { get; }

    public ValidationException()
        : base()
    {
        Errors = [];
    }

    public ValidationException(string message)
        : base(message)
    {
        Errors = [];
    }

    public ValidationException(string message, Exception innerException)
        : base(message, innerException)
    {
        Errors = [];
    }

    public ValidationException(IEnumerable<ValidationExceptionModel> errors) : base(BuildErrorMessage(errors))
    {
        Errors = errors;
    }

    private static string BuildErrorMessage(IEnumerable<ValidationExceptionModel> errors)
    {
        var arr = errors.Select(
            x =>
                $"{Environment.NewLine} -- {x.Property}: {string.Join(Environment.NewLine, values: x.Errors 
                    ?? Array.Empty<string>())}"
        );
        return $"Validation failed: {string.Join(string.Empty, arr)}";
    }
}