namespace MGH.Core.CrossCutting.Exceptions.Types;

public class ValidationExceptionModel
{
    public string Property { get; set; }
    public IEnumerable<string> Errors { get; set; }
}