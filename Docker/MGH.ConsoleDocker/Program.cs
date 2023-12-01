using MGH.ConsoleDocker;
using MGH.ConsoleDocker.Services;
using Microsoft.Extensions.DependencyInjection;

ServiceHelper.RegisterServices();
var scope = ServiceHelper.MyServiceProvider.CreateScope();
scope.ServiceProvider.GetRequiredService<ILogger>().LogInfo("Hello, docker!");

var personService = scope.ServiceProvider.GetRequiredService<IPersonService>();
personService.PrintFullName("ali", "ahmadi");

//important note : We should not use this two methods : 1-GetRequiredService 2- GetService  as much as possible.
//we should use ioc container in constructor. if that was impossible the preferred method is GetRequiredService.  
//when a service is registered out of the IOC container you must dispose of it manually with an IDisposable interface.

