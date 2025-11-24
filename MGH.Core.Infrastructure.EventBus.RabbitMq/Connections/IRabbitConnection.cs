using RabbitMQ.Client;

namespace MGH.Core.Infrastructure.EventBus.RabbitMq.Connections
{
    /// <summary>
    /// Represents a RabbitMQ connection manager that provides channels for publishing, consuming, and declaring.
    /// Handles connection management, reconnections, and disposal.
    /// </summary>
    public interface IRabbitConnection : IDisposable
    {
        /// <summary>
        /// Ensures the RabbitMQ connection is established.
        /// If the connection is closed or lost, it will attempt to reconnect.
        /// </summary>
        void ConnectService();

        /// <summary>
        /// Creates and returns a channel for publishing messages to RabbitMQ.
        /// </summary>
        /// <returns>An open <see cref="IModel"/> channel for publishing.</returns>
        IModel GetPublishChannel();

        /// <summary>
        /// Creates and returns a channel for consuming messages from RabbitMQ.
        /// </summary>
        /// <returns>An open <see cref="IModel"/> channel for consuming.</returns>
        IModel GetConsumeChannel();

        /// <summary>
        /// Creates and returns a channel for declaring exchanges, queues, and bindings.
        /// </summary>
        /// <returns>An open <see cref="IModel"/> channel for declarations.</returns>
        IModel GetDeclarerChannel();
    }
}
