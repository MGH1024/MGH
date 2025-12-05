using Microsoft.Extensions.Logging;
using Polly;

namespace MGH.Core.Infrastructure.EventBus.RabbitMq.Connections
{
    public class RabbitMqRetryPolicyProvider : IRabbitMqRetryPolicyProvider
    {
        private readonly AsyncPolicy _connectionPolicy;

        public RabbitMqRetryPolicyProvider(ILogger<RabbitMqRetryPolicyProvider> logger)
        {
            // Create and cache policy once (recommended by Polly)
            _connectionPolicy = PollyRabbitExtensions.CreateRabbitMqConnectionRetryPolicy(
                logger: logger,
                retryCount: 10,
                baseDelaySeconds: 5,
                maxDelaySeconds: 30
            );
        }

        public AsyncPolicy GetConnectionPolicy() => _connectionPolicy;
    }
}



