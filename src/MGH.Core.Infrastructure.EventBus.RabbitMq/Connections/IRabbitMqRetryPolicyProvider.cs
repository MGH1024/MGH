using Polly;
using Polly.Retry;
using Microsoft.Extensions.Logging;

namespace MGH.Core.Infrastructure.EventBus.RabbitMq.Connections
{
    public interface IRabbitMqRetryPolicyProvider
    {
        AsyncPolicy GetConnectionPolicy();
    }

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

    public static class PollyRabbitExtensions
    {
        public static AsyncRetryPolicy CreateRabbitMqConnectionRetryPolicy(
            ILogger logger,
            int retryCount = 10,
            int baseDelaySeconds = 5,
            int maxDelaySeconds = 30)
        {
            var jitter = new Random();

            return Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(
                    retryCount,
                    attempt =>
                    {
                        var delay = baseDelaySeconds * attempt;
                        delay = Math.Min(delay, maxDelaySeconds);
                        var jitterMs = jitter.Next(0, 500);

                        return TimeSpan.FromSeconds(delay)
                                       .Add(TimeSpan.FromMilliseconds(jitterMs));
                    },
                    (exception, delay, attempt, context) =>
                    {
                        logger.LogWarning(exception,
                            "RabbitMQ connection attempt {Attempt} failed. Retrying in {DelaySeconds}s...",
                            attempt, delay.TotalSeconds);
                    });
        }
    }
}



