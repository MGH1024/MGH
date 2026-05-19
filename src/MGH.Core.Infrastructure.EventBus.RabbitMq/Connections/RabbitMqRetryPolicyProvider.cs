using Polly;
using Microsoft.Extensions.Logging;

namespace MGH.Core.Infrastructure.EventBus.RabbitMq.Connections
{
    public class RabbitMqRetryPolicyProvider(ILogger<RabbitMqRetryPolicyProvider> logger)
        : IRabbitMqRetryPolicyProvider
    {
        private readonly AsyncPolicy _connectionPolicy = PollyRabbitExtensions.CreateRabbitMqConnectionRetryPolicy(
            logger: logger,
            retryCount: 10,
            baseDelaySeconds: 5,
            maxDelaySeconds: 30
        );

        public AsyncPolicy GetConnectionPolicy() => _connectionPolicy;
    }
}



