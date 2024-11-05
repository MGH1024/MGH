using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MGH.Core.Application.HttpClients.Base;

public class BaseHttpClient(HttpClient httpClient)
{
    private string Token { get; set; }
    private DateTime TokenExpiry { get; set; }

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

        var response = await httpClient.PostAsync(endpoint, content);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
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


    protected virtual async Task LoginAsync()
    {
        if (IsTokenActive()) return;

        Token = "abcdtyrsdfjjfjsdfjll45345345-dfdffsd-sdfsdfsdsd";
        TokenExpiry = DateTime.Now.AddSeconds(3600);
    }

    protected bool IsTokenActive() => !string.IsNullOrEmpty(Token) && DateTime.Now <= TokenExpiry;
}