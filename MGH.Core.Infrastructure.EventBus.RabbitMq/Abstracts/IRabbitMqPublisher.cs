namespace MGH.Core.Infrastructure.EventBus.RabbitMq.Abstracts;

public interface IRabbitMqPublisher
{
    void Publish<T>(T model);
}