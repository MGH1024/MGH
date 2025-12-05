using Polly;
using RabbitMQ.Client;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

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
        private IConnection? _connection;
        private AsyncPolicy? _connectionPolicy;
        private volatile bool _isConnecting = false;
        private readonly ILogger<RabbitConnection> _logger;
        private readonly ConnectionFactory _connectionFactory;
        private readonly SemaphoreSlim _connectionLock = new SemaphoreSlim(1, 1);

        /// <summary>
        /// Initializes a new instance of <see cref="RabbitConnection"/>.
        /// Validates configuration and establishes the initial connection to RabbitMQ.
        /// </summary>
        /// <param name="options">The RabbitMQ connection options.</param>
        /// <param name="logger">Logger for connection and error events.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="options"/> or <paramref name="logger"/> is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown if required RabbitMQ configuration is missing or invalid.</exception>
        public RabbitConnection(
            ILogger<RabbitConnection> logger,
            IOptions<RabbitMqOptions> options,
            IRabbitMqRetryPolicyProvider retryPolicyProvider)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            if(options is null)
                throw new ArgumentNullException(nameof(options), "RabbitMQ options are missing.");

            var cfg = options.Value;
            var connCfg = cfg.Connections?.Default;
            if (connCfg == null)
                throw new InvalidOperationException("Default connection configuration is missing.");

            if (string.IsNullOrWhiteSpace(connCfg.Host))
                throw new InvalidOperationException("RabbitMQ host is not configured.");

            if (string.IsNullOrWhiteSpace(connCfg.Username))
                throw new InvalidOperationException("RabbitMQ username is not configured.");

            if (string.IsNullOrWhiteSpace(connCfg.Password))
                throw new InvalidOperationException("RabbitMQ password is not configured.");

            if (string.IsNullOrWhiteSpace(connCfg.VirtualHost))
                throw new InvalidOperationException("RabbitMQ virtualHost is not configured.");

            if (string.IsNullOrWhiteSpace(connCfg.Port))
                throw new InvalidOperationException("RabbitMQ port is not configured.");

            if (!int.TryParse(connCfg.Port, out int port) || port <= 0)
                throw new InvalidOperationException($"RabbitMQ port is invalid. Must be a positive integer.");

            if ( connCfg.ConsumerDispatchConcurrency <= 0)
                throw new InvalidOperationException($"RabbitMQ consumerDispatchConcurrency is invalid. Must be a positive integer.");

            _connectionFactory = new ConnectionFactory
            {
                HostName = connCfg.Host,
                Port = port,
                UserName = connCfg.Username,
                Password = connCfg.Password,
                VirtualHost = connCfg.VirtualHost,
                AutomaticRecoveryEnabled = connCfg.AutomaticRecoveryEnabled,
                ConsumerDispatchConcurrency = connCfg.ConsumerDispatchConcurrency,
            };

            _logger.LogInformation("RabbitMQ connection factory initialized for host '{Host}' on port {Port}.",
                _connectionFactory.HostName, _connectionFactory.Port);

            _connectionPolicy = retryPolicyProvider.GetConnectionPolicy();
        }

        /// <summary>
        /// Ensures the RabbitMQ connection is established.
        /// Reconnects automatically if the connection is lost or encounters an exception.
        /// </summary>
        /// <remarks>
        /// Uses the Polly retry policy to handle transient errors.
        /// Logs connection events and retries.
        /// </remarks>
        public async Task ConnectServiceAsync()
        {
            if (_connectionPolicy is null)
                throw new InvalidOperationException("Connection policy must be configured.");

            if (_isDisposed) return;

            await _connectionLock.WaitAsync().ConfigureAwait(false);
            try
            {
                if (_isConnecting) return;
                if (_connection != null && _connection.IsOpen) return;

                _isConnecting = true;

                await _connectionPolicy.ExecuteAsync(async () =>
                {
                    if (_isDisposed) return;
                    if (_connection != null && _connection.IsOpen) return;

                    try
                    {
                        _connection?.Dispose();

                        _connection = await _connectionFactory
                            .CreateConnectionAsync(clientProvidedName: "RabbitMQ_Connection")
                            .ConfigureAwait(false);

                        if (!_connection.IsOpen)
                            throw new InvalidOperationException("RabbitMQ connection could not be opened.");

                        _logger.LogInformation("RabbitMQ connection established successfully.");

                        _connection.ConnectionShutdownAsync += async (s, e) =>
                        {
                            if (_isDisposed) return;
                            _logger.LogWarning("RabbitMQ connection shutdown detected. Reconnecting...");
                            _ = Task.Run(() => ConnectServiceAsync());
                        };

                        _connection.CallbackExceptionAsync += async (s, e) =>
                        {
                            if (_isDisposed) return;
                            _logger.LogWarning(e.Exception, "RabbitMQ callback exception. Reconnecting...");
                            _ = Task.Run(() => ConnectServiceAsync());
                        };

                        _connection.ConnectionBlockedAsync += async (s, e) =>
                        {
                            if (_isDisposed) return;
                            _logger.LogWarning("RabbitMQ connection blocked. Reconnecting...");
                            _ = Task.Run(() => ConnectServiceAsync());
                        };
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "RabbitMQ connection attempt failed. Will retry...");
                        throw;
                    }
                    finally
                    {
                        _isConnecting = false;
                    }
                }).ConfigureAwait(false);
            }
            finally
            {
                _connectionLock.Release();
            }
        }

        /// <summary>
        /// Returns a channel for publishing messages to RabbitMQ.
        /// </summary>
        /// <returns>A new <see cref="IModel"/> for publishing.</returns>
        public async Task<IChannel> GetPublishChannelAsync() => await CreateChannelAsync();

        /// <summary>
        /// Returns a channel for consuming messages from RabbitMQ.
        /// </summary>
        /// <returns>A new <see cref="IModel"/> for consuming.</returns>
        public async Task<IChannel> GetConsumeChannelAsync() => await CreateChannelAsync();

        /// <summary>
        /// Returns a channel for declaring exchanges and queues in RabbitMQ.
        /// </summary>
        /// <returns>A new <see cref="IModel"/> for declaration operations.</returns>
        public async Task<IChannel> GetDeclarerChannelAsync() => await CreateChannelAsync();

        private async Task<IChannel> CreateChannelAsync()
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(RabbitConnection));

            // Ensure connection exists and is open
            if (_connection == null || !_connection.IsOpen)
            {
                await ConnectServiceAsync().ConfigureAwait(false);
            }

            if (_connection == null || !_connection.IsOpen)
                throw new InvalidOperationException("RabbitMQ connection is not available.");

            IChannel? channel = null;

            try
            {
                // Attempt to create a channel
                channel = await _connection.CreateChannelAsync().ConfigureAwait(false);

                if (channel == null)
                    throw new InvalidOperationException("Failed to create a RabbitMQ channel.");

                // Subscribe to callback exceptions to trigger reconnect
                channel.CallbackExceptionAsync += async (s, e) =>
                {
                    if (_isDisposed) return;

                    try
                    {
                        _logger.LogWarning(e.Exception, "RabbitMQ channel callback exception. Reconnecting...");
                        await ConnectServiceAsync().ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error during channel reconnect after callback exception.");
                    }
                };

                return channel;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create RabbitMQ channel.");
                channel?.Dispose(); // Dispose partially created channel if possible
                throw;
            }
        }

        public void Dispose()
        {
            if (_isDisposed) return;

            try
            {
                _connectionLock?.Dispose();
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