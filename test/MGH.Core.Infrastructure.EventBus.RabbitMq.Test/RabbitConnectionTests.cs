using Moq;
using RabbitMQ.Client;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MGH.Core.Infrastructure.EventBus.RabbitMq.Connections;
using Polly;

namespace MGH.Core.Infrastructure.EventBus.RabbitMq.Test;


public class RabbitConnectionTests
{
    private readonly RabbitMqOptions _rabbitMqOptions;
    private readonly Mock<ILogger<RabbitConnection>> _mocklogger;
    private readonly Mock<IOptions<RabbitMqOptions>> _mockRabbitMqOptions;
    private readonly Mock<IRabbitMqRetryPolicyProvider> _mockrabbitMqRetryPolicyProvider;

    public RabbitConnectionTests()
    {
        _rabbitMqOptions = new RabbitMqOptions
        {
            Connections = new ConnectionOptions
            {
                Default = new RabbitMqSettings
                {
                    Host = "localhost",
                    Password = "password",
                    AutomaticRecoveryEnabled = true,
                    ConsumerDispatchConcurrency = 1,
                    Port = "5672",
                    Username = "guest",
                    VirtualHost = "/",
                    ReceiveEndpoint = ""
                }
            },
            EventBus = new EventBusOptions
            {
                QueueName = "test",
                ExchangeName = "test",
                ExchangeType = "test",
            }
        };

        _mockRabbitMqOptions = new Mock<IOptions<RabbitMqOptions>>();
        _mockRabbitMqOptions
           .Setup(o => o.Value)
           .Returns(_rabbitMqOptions);

        _mocklogger = new Mock<ILogger<RabbitConnection>>();
        _mockrabbitMqRetryPolicyProvider = new Mock<IRabbitMqRetryPolicyProvider>();
    }

    [Fact]
    public void Constructor_NullLogger_ThrowsArgumentNullException()
    {
        // Arrange
        //optionMocked in constructor

        // Act & Assert
        var ex = Assert.Throws<ArgumentNullException>(() =>
            new RabbitConnection(
                null!,
                _mockRabbitMqOptions.Object,
                _mockrabbitMqRetryPolicyProvider.Object));

        Assert.Equal("logger", ex.ParamName);
    }

    [Fact]
    public void Constructor_ValidLogger_DoesNotThrow()
    {
        // Arrange
        //optionMocked in constructor

        var _rabbitMqRetryPolicyProvider = new Mock<IRabbitMqRetryPolicyProvider>();

        // Act & Assert
        var connection = new RabbitConnection(
            _mocklogger.Object,
            _mockRabbitMqOptions.Object,
            _rabbitMqRetryPolicyProvider.Object);

        Assert.NotNull(connection);
    }

    [Fact]
    public void Constructor_NullOptions_ThrowsArgumentNullException()
    {
        // Arrange

        // Act & Assert
        var ex = Assert.Throws<ArgumentNullException>(() =>
            new RabbitConnection(
                _mocklogger.Object,
                null!,
                _mockrabbitMqRetryPolicyProvider.Object));

        Assert.Equal("options", ex.ParamName);
        Assert.Contains("RabbitMQ options are missing", ex.Message);
    }

    [Fact]
    public void Constructor_ValidOptions_DoesNotThrow()
    {
        // Arrange
        var optionsMock = new Mock<IOptions<RabbitMqOptions>>();
        optionsMock.Setup(o => o.Value).Returns(_rabbitMqOptions);

        // Act
        var connection = new RabbitConnection(
            _mocklogger.Object,
            optionsMock.Object,
            _mockrabbitMqRetryPolicyProvider.Object);

        // Assert
        Assert.NotNull(connection);
    }

    [Fact]
    public void Constructor_ValidConnection_DoesNotThrow()
    {
        // Arrange
        var optionsMock = new Mock<IOptions<RabbitMqOptions>>();
        optionsMock.Setup(o => o.Value).Returns(_rabbitMqOptions);

        // Act & Assert
        var connection = new RabbitConnection(
            _mocklogger.Object,
            optionsMock.Object,
            _mockrabbitMqRetryPolicyProvider.Object);
        Assert.NotNull(connection);
    }

    [Fact]
    public void Constructor_NullDefaultConnection_ThrowsInvalidOperationException()
    {
        // Arrange
        var optionsMock = new Mock<IOptions<RabbitMqOptions>>();
        _rabbitMqOptions.Connections.Default = null;
        optionsMock.Setup(o => o.Value).Returns(_rabbitMqOptions);

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() =>
            new RabbitConnection(
                _mocklogger.Object,
                optionsMock.Object,
                _mockrabbitMqRetryPolicyProvider.Object));

        Assert.Equal("Default connection configuration is missing.", ex.Message);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Constructor_EmptyHost_ThrowsInvalidOperationException(string host)
    {
        // Arrange
        var optionsMock = new Mock<IOptions<RabbitMqOptions>>();
        _rabbitMqOptions.Connections.Default.Host = host;
        optionsMock.Setup(o => o.Value).Returns(_rabbitMqOptions);

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() =>
            new RabbitConnection(
                _mocklogger.Object,
                optionsMock.Object,
                _mockrabbitMqRetryPolicyProvider.Object));

        Assert.Equal("RabbitMQ host is not configured.", ex.Message);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Constructor_EmptyUsername_ThrowsInvalidOperationException(string username)
    {
        // Arrange
        var optionsMock = new Mock<IOptions<RabbitMqOptions>>();
        _rabbitMqOptions.Connections.Default.Username = username;
        optionsMock.Setup(o => o.Value).Returns(_rabbitMqOptions);

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() =>
            new RabbitConnection(
                _mocklogger.Object,
                optionsMock.Object,
                _mockrabbitMqRetryPolicyProvider.Object));

        Assert.Equal("RabbitMQ username is not configured.", ex.Message);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Constructor_EmptyPassword_ThrowsInvalidOperationException(string password)
    {
        // Arrange
        var optionsMock = new Mock<IOptions<RabbitMqOptions>>();
        _rabbitMqOptions.Connections.Default.Password = password;
        optionsMock.Setup(o => o.Value).Returns(_rabbitMqOptions);

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() =>
            new RabbitConnection(
                _mocklogger.Object,
                optionsMock.Object,
                _mockrabbitMqRetryPolicyProvider.Object
            ));

        Assert.Equal("RabbitMQ password is not configured.", ex.Message);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Constructor_EmptyVirtualHost_ThrowsInvalidOperationException(string virtualHost)
    {
        // Arrange
        var optionsMock = new Mock<IOptions<RabbitMqOptions>>();
        _rabbitMqOptions.Connections.Default.VirtualHost = virtualHost;
        optionsMock.Setup(o => o.Value).Returns(_rabbitMqOptions);

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() =>
            new RabbitConnection(
                _mocklogger.Object,
                optionsMock.Object,
                _mockrabbitMqRetryPolicyProvider.Object));

        Assert.Equal("RabbitMQ virtualHost is not configured.", ex.Message);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Constructor_EmptyPort_ThrowsInvalidOperationException(string port)
    {
        // Arrange
        var optionsMock = new Mock<IOptions<RabbitMqOptions>>();
        _rabbitMqOptions.Connections.Default.Port = port;
        optionsMock.Setup(o => o.Value).Returns(_rabbitMqOptions);

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() =>
            new RabbitConnection(
                _mocklogger.Object,
                optionsMock.Object,
                _mockrabbitMqRetryPolicyProvider.Object));

        Assert.Equal("RabbitMQ port is not configured.", ex.Message);
    }

    [Theory]
    [InlineData("-1")]
    public void Constructor_NegativePort_ThrowsInvalidOperationException(string port)
    {
        // Arrange
        var optionsMock = new Mock<IOptions<RabbitMqOptions>>();
        _rabbitMqOptions.Connections.Default.Port = port;
        optionsMock.Setup(o => o.Value).Returns(_rabbitMqOptions);

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() =>
            new RabbitConnection(
                _mocklogger.Object,
                optionsMock.Object,
                _mockrabbitMqRetryPolicyProvider.Object));

        Assert.Equal("RabbitMQ port is invalid. Must be a positive integer.", ex.Message);
    }

    [Theory]
    [InlineData(0)]
    public void Constructor_NegativeConsumerDispatchConcurrency_ThrowsInvalidOperationException(int consumerDispatchConcurrency)
    {
        // Arrange
        var optionsMock = new Mock<IOptions<RabbitMqOptions>>();
        _rabbitMqOptions.Connections.Default.ConsumerDispatchConcurrency = (ushort)consumerDispatchConcurrency;
        optionsMock.Setup(o => o.Value).Returns(_rabbitMqOptions);

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() =>
            new RabbitConnection(
                _mocklogger.Object,
                optionsMock.Object,
                _mockrabbitMqRetryPolicyProvider.Object));

        Assert.Equal("RabbitMQ consumerDispatchConcurrency is invalid. Must be a positive integer.", ex.Message);
    }

    [Fact]
    public void Constructor_ValidOption_ShouldInitiateConnectionFactory()
    {
        // Arrange
        var expectedOptions = _rabbitMqOptions;

        var optionsMock = new Mock<IOptions<RabbitMqOptions>>();
        optionsMock.Setup(o => o.Value).Returns(expectedOptions);

        var loggerMock = new Mock<ILogger<RabbitConnection>>();

        // Act
        var sut = new RabbitConnection(
            loggerMock.Object,
            optionsMock.Object,
            _mockrabbitMqRetryPolicyProvider.Object);

        // Access the private field _connectionFactory through reflection
        var factoryField = typeof(RabbitConnection)
            .GetField("_connectionFactory", BindingFlags.NonPublic | BindingFlags.Instance);

        var factory = (ConnectionFactory)factoryField!.GetValue(sut)!;

        // Assert
        Assert.Equal(expectedOptions.Connections.Default.Host, factory.HostName);
        Assert.Equal(expectedOptions.Connections.Default.Port, factory.Port.ToString());
        Assert.Equal(expectedOptions.Connections.Default.Username, factory.UserName);
        Assert.Equal(expectedOptions.Connections.Default.Password, factory.Password);
        Assert.Equal(expectedOptions.Connections.Default.VirtualHost, factory.VirtualHost);
        Assert.Equal(expectedOptions.Connections.Default.AutomaticRecoveryEnabled, factory.AutomaticRecoveryEnabled);
        Assert.Equal(expectedOptions.Connections.Default.ConsumerDispatchConcurrency, factory.ConsumerDispatchConcurrency);
    }

    [Fact]
    public async Task ConnectServiceAsync_ConnectionPolicy_ThrowsInvalidOperationException()
    {
        // Arrange
        _mockrabbitMqRetryPolicyProvider
            .Setup(x => x.GetConnectionPolicy())
            .Returns((AsyncPolicy)null);

        var rabbitMqConnection = new RabbitConnection(
            _mocklogger.Object,
            _mockRabbitMqOptions.Object,
            _mockrabbitMqRetryPolicyProvider.Object
        );

        // Act
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => rabbitMqConnection.ConnectServiceAsync()
        );

        // Assert
        Assert.Equal("Connection policy must be configured.", exception.Message);
    }

}