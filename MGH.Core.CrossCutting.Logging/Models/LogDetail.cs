namespace MGH.Core.CrossCutting.Logging.Models;

public class LogDetail(string fullName, string methodName, string user, List<LogParameter> parameters)
{
    public string FullName { get; set; } = fullName;
    public string MethodName { get; set; } = methodName;
    public string User { get; set; } = user;
    public List<LogParameter> Parameters { get; set; } = parameters;

    public LogDetail() : this(string.Empty, string.Empty, string.Empty, new List<LogParameter>())
    {
    }
}
