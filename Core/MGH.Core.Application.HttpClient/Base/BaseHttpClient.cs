using System.Text.Json;

namespace MGH.Core.Application.HttpClients.Base;

public class BaseHttpClient(HttpClient httpClient)
{
    protected async Task<T> GetAsync<T>(string endpoint)
    {
        var response = await httpClient.GetAsync(endpoint);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(json);
    }
}