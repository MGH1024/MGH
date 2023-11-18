namespace MGH.ConsoleDocker.Services;

public class Logger : ILogger
{
    public void LogInfo(string text)
    {
        Console.WriteLine(text);
    }
}