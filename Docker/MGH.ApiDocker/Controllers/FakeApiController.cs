using MGH.ApiDocker.Services;
using Microsoft.AspNetCore.Mvc;

namespace MGH.ApiDocker.Controllers;

[ApiController]
[Route("api/fake")]
public class FakeApiController : ControllerBase
{
    private readonly IHttpClientFakeService _httpClientFakeService;
    private readonly IHttpNamedFakeService _httpNamedFakeService;

    public FakeApiController(IHttpClientFakeService httpClientFakeService, IHttpNamedFakeService httpNamedFakeService)
    {
        _httpClientFakeService = httpClientFakeService;
        _httpNamedFakeService = httpNamedFakeService;
    }

    [HttpGet("users-http-client")]
    public async Task<IActionResult> GetUsersFromHttpClientAsync()
    {
        return await GetResponseAsync(() => _httpClientFakeService.GetUsers());
    }

    [HttpGet("users-named-http-client")]
    public async Task<IActionResult> GetUsersFromNamedHttpClientAsync()
    {
        return await GetResponseAsync(() => _httpNamedFakeService.GetUsers());
    }

    [HttpGet("users-named-http-client/{id}")]
    public async Task<IActionResult> GetUserByIdFromNamedHttpClientAsync(int id)
    {
        return await GetResponseAsync(() => _httpNamedFakeService.GetUserById(id));
    }

    // Helper method to reduce duplication
    private async Task<IActionResult> GetResponseAsync<T>(Func<Task<T>> fetchData)
    {
        var data = await fetchData();
        return Ok(data);
    }
}