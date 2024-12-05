using RabbitMQ.Client;

namespace MGH.Core.Infrastructure.EventBus.RabbitMq.Abstracts;

public interface IRabbitMqConnection : IDisposable
{
    void ConnectService();
    IModel GetChannel();
}