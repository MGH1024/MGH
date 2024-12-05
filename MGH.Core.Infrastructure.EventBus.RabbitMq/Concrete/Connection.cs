using MGH.Core.Infrastructure.EventBus.RabbitMq.Abstracts;
using Microsoft.Extensions.Options;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MGH.Core.Infrastructure.EventBus.RabbitMq.Concrete;

public class Connection : IRabbitMqConnection
{
    private Policy _connectionPolicy;
    private ConnectionFactory _connectionFactory;
    private IConnection _connection;
    private IModel _channel;
    private bool _isDisposed;
    private bool IsServiceConnected => _connection is not null && _connection.IsOpen;
    private bool IsChannelConnected => _channel is not null && _channel.IsOpen;

    public Connection(IOptions<Model.RabbitMq> options)
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

    private void CreateConnectionFactory(Model.RabbitMq rabbitMq)
    {
        _connectionFactory = new ConnectionFactory
        {
            UserName = rabbitMq.DefaultConnection.Username,
            Password = rabbitMq.DefaultConnection.Password,
            VirtualHost = rabbitMq.DefaultConnection.VirtualHost,
            HostName = rabbitMq.DefaultConnection.Host,
            Port = Convert.ToInt32(rabbitMq.DefaultConnection.Port),
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