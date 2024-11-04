using System.Text.Json;
using MGH.ApiDocker.Models;

namespace MGH.ApiDocker.Services;

public class HttpClientIHttpClientFakeService(HttpClient httpClient) : IHttpClientFakeService
{
    public async Task<IEnumerable<User>> GetUsers()
    {
        var response =await httpClient.GetAsync("/users");
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<IEnumerable<User>>(json);
    }
}