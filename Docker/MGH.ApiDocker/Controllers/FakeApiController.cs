using MGH.ApiDocker.Services;
using Microsoft.AspNetCore.Mvc;

namespace MGH.ApiDocker.Controllers;

public class FakeApiController : ControllerBase
{
    private readonly IHttpClientFakeService _iHttpClientFakeService;
    private readonly IHttpNamedFakeService _iHttpNamedFakeService;

    public FakeApiController(IHttpClientFakeService iHttpClientFakeService, IHttpNamedFakeService iHttpNamedFakeService)
    {
        _iHttpClientFakeService = iHttpClientFakeService;
        _iHttpNamedFakeService = iHttpNamedFakeService;
    }

    [HttpGet("get-users-http-client")]
    public async Task<IActionResult> GetUsersAsync()
    {
        return Ok(await _iHttpClientFakeService.GetUsers());
    }
    
    [HttpGet("get-users-named-http-client")]
    public async Task<IActionResult> GetUsers2Async()
    {
        return Ok(await _iHttpNamedFakeService.GetUsers());
    }
}