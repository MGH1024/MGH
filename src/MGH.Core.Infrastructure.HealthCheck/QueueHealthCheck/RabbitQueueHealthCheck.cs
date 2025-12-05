using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace MGH.Core.Infrastructure.HealthCheck.QueueHealthCheck;

public class RabbitQueueHealthCheck : IHealthCheck
{
    private readonly string _queueName;
    private readonly IConfiguration _configuration;
    private readonly Func<int, bool> _degradedCondition;
    private readonly Func<int, bool> _unhealthyCondition;

    public RabbitQueueHealthCheck(IConfiguration configuration, string queueName,
        Func<int, bool> degradedCondition,
        Func<int, bool> unhealthyCondition)
    {
        _configuration = configuration;
        _queueName = queueName;
        _degradedCondition = degradedCondition;
        _unhealthyCondition = unhealthyCondition;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var RetryMessageCount = new RabbitMQManagementHttpClient(_configuration)
                                    .GetRetryMessageCountAsync(_queueName, cancellationToken);

            var message = $"Messages count in queue : {_queueName}  is {RetryMessageCount.Result}.";

            if (_unhealthyCondition.Invoke(RetryMessageCount.Result))
                return HealthCheckResult.Unhealthy(message);

            if (_degradedCondition.Invoke(RetryMessageCount.Result))
                return HealthCheckResult.Degraded(message);

            return HealthCheckResult.Healthy(message);
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Get Retry Message Count is not work.", ex);
        }
    }
}
