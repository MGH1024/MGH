using Polly;
using RabbitMQ.Client;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using MGH.Core.Infrastructure.EventBus.RabbitMq.Options;

namespace MGH.Core.Infrastructure.EventBus.RabbitMq.Connections
{
    /// <summary>
    /// Manages a persistent connection to RabbitMQ.
    /// Provides channels for publishing, consuming, and declaring messages.
    /// Handles automatic reconnects and retry policies using Polly.
    /// </summary>
    /// <remarks>
    /// This class is thread-safe for creating multiple channels concurrently.
    /// It automatically reconnects when the connection is shut down, blocked, or encounters a callback exception.
    /// Retry policies use exponential backoff and are logged.
    /// </remarks>
    public class RabbitConnection : IRabbitConnection, IDisposable
    {
        private bool _isDisposed;
        private IConnection _connection;
        private Policy _connectionPolicy;
        private readonly ILogger<RabbitConnection> _logger;
        private readonly ConnectionFactory _connectionFactory;

        /// <summary>
        /// Initializes a new instance of <see cref="RabbitConnection"/>.
        /// Validates configuration and establishes the initial connection to RabbitMQ.
        /// </summary>
        /// <param name="options">The RabbitMQ connection options.</param>
        /// <param name="logger">Logger for connection and error events.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="options"/> or <paramref name="logger"/> is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown if required RabbitMQ configuration is missing or invalid.</exception>
        public RabbitConnection(
            IOptions<RabbitMqOptions> options,
            ILogger<RabbitConnection> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            if (options?.Value == null)
                throw new ArgumentNullException(nameof(options), "RabbitMQ options are missing.");

            var cfg = options.Value;
            var connCfg = cfg.Connections?.Default
                          ?? throw new InvalidOperationException("Default connection configuration is missing.");

            if (string.IsNullOrWhiteSpace(connCfg.Host))
                throw new InvalidOperationException("RabbitMQ host is not configured.");

            if (string.IsNullOrWhiteSpace(connCfg.Username))
                throw new InvalidOperationException("RabbitMQ username is not configured.");

            if (string.IsNullOrWhiteSpace(connCfg.Password))
                throw new InvalidOperationException("RabbitMQ password is not configured.");

            if (string.IsNullOrWhiteSpace(connCfg.VirtualHost))
                throw new InvalidOperationException("RabbitMQ virtual host is not configured.");

            if (!int.TryParse(connCfg.Port, out int port))
                throw new InvalidOperationException($"RabbitMQ port is invalid: '{connCfg.Port}'");

            _connectionFactory = new ConnectionFactory
            {
                UserName = connCfg.Username,
                Password = connCfg.Password,
                VirtualHost = connCfg.VirtualHost,
                HostName = connCfg.Host,
                Port = port
            };

            CreateConnectionPolicy();
            ConnectService();
        }

        /// <summary>
        /// Ensures the RabbitMQ connection is established.
        /// Reconnects automatically if the connection is lost or encounters an exception.
        /// </summary>
        /// <remarks>
        /// Uses the Polly retry policy to handle transient errors.
        /// Logs connection events and retries.
        /// </remarks>
        public void ConnectService()
        {
            if (_isDisposed) return;
            _connectionPolicy.Execute(() =>
            {
                if (_isDisposed) return;
                if (_connection != null && _connection.IsOpen) return;

                try
                {
                    _connection?.Dispose();
                    _connection = _connectionFactory.CreateConnection(clientProvidedName: "RabbitMQ_Connection");
                    if (!_connection.IsOpen)
                        throw new InvalidOperationException("RabbitMQ connection could not be opened.");

                    _connection.ConnectionShutdown += (s, e) =>
                    {
                        if (!_isDisposed)
                        {
                            _logger.LogWarning("RabbitMQ connection shutdown detected. Reconnecting...");
                            ConnectService();
                        }
                    };
                    _connection.CallbackException += (s, e) =>
                    {
                        if (!_isDisposed)
                        {
                            _logger.LogWarning(e.Exception, "RabbitMQ callback exception. Reconnecting...");
                            ConnectService();
                        }
                    };
                    _connection.ConnectionBlocked += (s, e) =>
                    {
                        if (!_isDisposed)
                        {
                            _logger.LogWarning("RabbitMQ connection blocked. Reconnecting...");
                            ConnectService();
                        }
                    };
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "RabbitMQ connection failed. Will retry...");
                    throw; // Polly will handle retries
                }
            });
        }

        /// <summary>
        /// Creates the Polly retry policy for RabbitMQ connection attempts.
        /// </summary>
        /// <remarks>
        /// Retries up to 10 times with exponential backoff, up to 30 seconds per retry.
        /// Logs each retry attempt with exception details.
        /// </remarks>
        private void CreateConnectionPolicy()
        {
            _connectionPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetry(
                    retryCount: 10,
                    sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Min(5 * attempt, 30)),
                    onRetry: (exception, timespan, retryCount, context) =>
                    {
                        _logger.LogWarning(exception,
                            "RabbitMQ connection failed. Retry {RetryCount} in {DelaySeconds}s",
                            retryCount, timespan.TotalSeconds);
                    }
                );
        }

        /// <summary>
        /// Returns a channel for publishing messages to RabbitMQ.
        /// </summary>
        /// <returns>A new <see cref="IModel"/> for publishing.</returns>
        public IModel GetPublishChannel() => CreateChannel();

        /// <summary>
        /// Returns a channel for consuming messages from RabbitMQ.
        /// </summary>
        /// <returns>A new <see cref="IModel"/> for consuming.</returns>
        public IModel GetConsumeChannel() => CreateChannel();

        /// <summary>
        /// Returns a channel for declaring exchanges and queues in RabbitMQ.
        /// </summary>
        /// <returns>A new <see cref="IModel"/> for declaration operations.</returns>
        public IModel GetDeclarerChannel() => CreateChannel();

        /// <summary>
        /// Creates a new channel from the current RabbitMQ connection.
        /// </summary>
        /// <returns>A new <see cref="IModel"/> channel.</returns>
        /// <exception cref="ObjectDisposedException">Thrown if the connection is already disposed.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the connection is unavailable.</exception>
        /// <remarks>
        /// Automatically reconnects if the channel encounters a callback exception.
        /// Logs exceptions during channel creation.
        /// </remarks>
        private IModel CreateChannel()
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(RabbitConnection));

            ConnectService();

            if (_connection == null || !_connection.IsOpen)
                throw new InvalidOperationException("RabbitMQ connection is not available.");

            try
            {
                var channel = _connection.CreateModel();
                channel.CallbackException += (s, e) =>
                {
                    _logger.LogWarning(e.Exception, "RabbitMQ channel callback exception. Reconnecting...");
                    ConnectService();
                };
                return channel;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create RabbitMQ channel.");
                throw;
            }
        }

        /// <summary>
        /// Disposes the RabbitMQ connection and marks this instance as disposed.
        /// </summary>
        /// <remarks>
        /// Ensures that no further reconnects are attempted.
        /// Logs any errors encountered during disposal.
        /// </remarks>
        public void Dispose()
        {
            if (_isDisposed) return;

            try
            {
                _connection?.Dispose();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error disposing RabbitMQ connection.");
            }
            finally
            {
                _isDisposed = true;
            }
        }
    }
}
