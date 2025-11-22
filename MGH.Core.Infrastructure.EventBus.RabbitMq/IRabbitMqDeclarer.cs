namespace MGH.Core.Infrastructure.EventBus.RabbitMq;

public interface IRabbitMqDeclarer
{
    void BindExchangesAndQueues();
    void EndToEndExchangeBinding();
}