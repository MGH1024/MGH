using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MGH.Core.Infrastructure.HttpClient.Base;

public abstract class BaseHttpClient(System.Net.Http.HttpClient httpClient)
{
    protected string Token { get; set; }
    protected DateTime TokenExpiry { get; set; }

    protected async Task<T> GetAsync<T>(string endpoint, bool isEnableAuth = false)
    {
        if (isEnableAuth)
        {
            if (!IsTokenActive())
            {
                await LoginAsync();
            }
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
        }

        var response = await httpClient.GetAsync(endpoint);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
            IgnoreReadOnlyProperties = true,
            MaxDepth = 64
        };
        try
        {
            return JsonSerializer.Deserialize<T>(json,options) ??
                   throw new InvalidOperationException("Deserialization returned null.");
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException($"Failed to deserialize response for endpoint {endpoint}.", ex);
        }
    }

    protected async Task<T> PostAsync<T>(string endpoint, object data, bool isEnableAuth = false,
        CancellationToken cancellationToken=default)
    {
        if (isEnableAuth)
        {
            if (!IsTokenActive())
            {
                await LoginAsync();
            }
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
        }

        var content = new StringContent(JsonSerializer.Serialize(data));
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        var response = await httpClient.PostAsync(endpoint, content,cancellationToken);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        try
        {
            return JsonSerializer.Deserialize<T>(json) ??
                   throw new InvalidOperationException("Deserialization returned null.");
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException($"Failed to deserialize response for endpoint {endpoint}.", ex);
        }
    }


    protected abstract  Task LoginAsync();

    protected bool IsTokenActive() => !string.IsNullOrEmpty(Token) && DateTime.Now <= TokenExpiry;
}