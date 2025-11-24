namespace MGH.Core.Infrastructure.EventBus.RabbitMq
{
    /// <summary>
    /// Responsible for declaring and binding RabbitMQ exchanges and queues.
    /// Handles both main and retry queues as well as end-to-end exchange bindings.
    /// </summary>
    public interface IRabbitMqDeclarer
    {
        /// <summary>
        /// Declares the main exchange and queue, sets up retry queues, 
        /// and binds queues to the appropriate routing keys.
        /// </summary>
        void BindExchangesAndQueues();

        /// <summary>
        /// Declares and binds end-to-end exchanges according to the configuration.
        /// This includes binding source exchanges to destination exchanges and 
        /// associating them with the main queue if necessary.
        /// </summary>
        void EndToEndExchangeBinding();
    }
}
