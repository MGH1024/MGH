using MGH.ConsoleDocker;
using MGH.ConsoleDocker.Services;
using Microsoft.Extensions.DependencyInjection;

ServiceHelper.RegisterServices();
var scope = ServiceHelper.MyServiceProvider.CreateScope();
scope.ServiceProvider.GetRequiredService<ILogger>().LogInfo("Hello, docker!");


