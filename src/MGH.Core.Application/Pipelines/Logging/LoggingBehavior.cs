using MediatR;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace MGH.Core.Application.Pipelines.Logging;

public class LoggingBehaviour<TRequest, TResponse>(ILogger<LoggingBehaviour<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Handling {Name}",
            typeof(TRequest).Name);

        var myType = request.GetType();
        IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
        foreach (PropertyInfo prop in props)
        {
            var propValue = prop.GetValue(
                request,
                null);
            logger.LogInformation(
                "{Property} : {@Value}",
                prop.Name,
                propValue);
        }

        var response = await next(cancellationToken);
        logger.LogInformation(
            "Handled {Name}", 
            typeof(TResponse).Name);
        return response;
    }
}