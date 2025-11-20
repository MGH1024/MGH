using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.Options;
using MGH.Core.Infrastructure.EventBus.RabbitMq.Configurations;

namespace MGH.Core.Infrastructure.EventBus.RabbitMq.Connections;

public class RabbitConnection : IRabbitConnection
{
    private IModel _channel;
    private bool _isDisposed;
    private IConnection _connection;
    private Policy _connectionPolicy;
    private ConnectionFactory _connectionFactory;
    private bool IsChannelConnected => _channel is not null && _channel.IsOpen;
    private bool IsServiceConnected => _connection is not null && _connection.IsOpen;

    public RabbitConnection(IOptions<RabbitMqOptions> options)
    {
        CreateConnectionPolicy();
        CreateConnectionFactory(options.Value);
        ConnectService();
    }

    public IModel GetChannel()
    {
        return _channel;
    }

    public void Dispose()
    {
        if (_isDisposed) return;
        _channel?.Dispose();
        _connection?.Dispose();

        _isDisposed = true;
    }

    public void ConnectService()
    {
        _connectionPolicy.Execute(() =>
        {
            if (!_isDisposed && !IsServiceConnected)
            {
                _connection?.Dispose();
                _connection = _connectionFactory.CreateConnection(clientProvidedName: "Publisher Connection");
                _connection.CallbackException += Connection_CallbackException;
                _connection.ConnectionBlocked += Connection_ConnectionBlocked;
                _connection.ConnectionShutdown += Connection_ConnectionShutdown;
            }

            ConnectChannel();
        });
    }

    private void CreateConnectionPolicy()
    {
        _connectionPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetry(
                retryCount: 1000,
                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(5)
            );
    }

    private void CreateConnectionFactory(RabbitMqOptions rabbitMQ)
    {
        _connectionFactory = new ConnectionFactory
        {
            UserName = rabbitMQ.Connections.Default.Username,
            Password = rabbitMQ.Connections.Default.Password,
            VirtualHost = rabbitMQ.Connections.Default.VirtualHost,
            HostName = rabbitMQ.Connections.Default.Host,
            Port = Convert.ToInt32(rabbitMQ.Connections.Default.Port),
        };
    }

    private void ConnectChannel()
    {
        if (_isDisposed || IsChannelConnected) return;
        _channel?.Dispose();

        _channel = _connection.CreateModel();
        _channel.CallbackException += Channel_CallbackException;
    }

    private void Connection_ConnectionShutdown(object sender, ShutdownEventArgs e) => ConnectService();

    private void Connection_ConnectionBlocked(object sender, ConnectionBlockedEventArgs e) => ConnectService();

    private void Connection_CallbackException(object sender, CallbackExceptionEventArgs e) => ConnectService();

    private void Channel_CallbackException(object sender, CallbackExceptionEventArgs e) => ConnectService();
}