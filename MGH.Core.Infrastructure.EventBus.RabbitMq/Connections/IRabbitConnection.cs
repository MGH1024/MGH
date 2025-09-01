using RabbitMQ.Client;

namespace MGH.Core.Infrastructure.EventBus.RabbitMq.Connections;

public interface IRabbitConnection :IDisposable
{
    void ConnectService();
    IModel GetChannel();
}