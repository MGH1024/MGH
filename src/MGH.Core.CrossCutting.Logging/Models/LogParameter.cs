namespace MGH.Core.CrossCutting.Logging.Models;

public class LogParameter(string name, object value, string type)
{
    public string Name { get; set; } = name;
    public object Value { get; set; } = value;
    public string Type { get; set; } = type;

    public LogParameter() : this(string.Empty, string.Empty, string.Empty)
    {
    }
}
