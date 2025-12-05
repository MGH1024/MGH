using System.Text;
using System.Text.Json;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;

namespace MGH.Core.Infrastructure.HealthCheck.QueueHealthCheck;

public class RabbitMQManagementHttpClient
{
    private readonly IConfiguration _configuration;

    public RabbitMQManagementHttpClient(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<int> GetRetryMessageCountAsync(string queueName, CancellationToken cancellationToken = default)
    {
        try
        {
            var host = _configuration["RabbitMq:Connections:Default:HostName"];
            var username = @_configuration["RabbitMq:Connections:Default:UserName"];
            var password = @_configuration["RabbitMq:Connections:Default:Password"];
            var port = _configuration["RabbitMq:Connections:Default:ApiPort"];
            var virtualHost = _configuration["RabbitMq:Connections:Default:VirtualHost"];
            string basicCredentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));

            using (HttpClient client = new())
            {
                var request = new HttpRequestMessage(HttpMethod.Get, new Uri($"http://{host}:{port}/api/queues/{virtualHost}/{queueName}"));
                request.Headers.Authorization = new AuthenticationHeaderValue("Basic", basicCredentials);
                var response = await client.SendAsync(request, cancellationToken);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync(cancellationToken);
                var queueDetailsResult =JsonSerializer.Deserialize<QueueDetailsResult>(result);
                return (int)queueDetailsResult.Messages;
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}


