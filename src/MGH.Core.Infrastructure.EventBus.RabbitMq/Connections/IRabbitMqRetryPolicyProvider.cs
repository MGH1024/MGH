using Polly;

namespace MGH.Core.Infrastructure.EventBus.RabbitMq.Connections
{
    public interface IRabbitMqRetryPolicyProvider
    {
        AsyncPolicy GetConnectionPolicy();
    }
}



