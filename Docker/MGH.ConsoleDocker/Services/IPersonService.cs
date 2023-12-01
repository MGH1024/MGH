using Microsoft.Extensions.DependencyInjection;

namespace MGH.ConsoleDocker.Services;

public interface IPersonService
{
    void PrintFullName(string name, string family);
}

public class PersonService : IPersonService
{
    private readonly ILogger _logger;

    public PersonService(IServiceProvider serviceProvider)
    {
        _logger = serviceProvider.GetRequiredService<ILogger>();
    }

    public void PrintFullName(string name, string family)
    {
        _logger.LogInfo($"the fullname is {name} {family} !");
    }
}