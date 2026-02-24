using Microsoft.AspNetCore.Mvc;
using NwsWeather.Api.Services;

namespace NwsWeather.Api.Controllers;

[ApiController]
[Route("api/locations")]
public sealed class LocationsController : ControllerBase
{
    private readonly IForecastService _svc;

    public LocationsController(IForecastService svc) => _svc = svc;

    [HttpGet("state/{stateCode}")]
    public async Task<IActionResult> GetByState(string stateCode, CancellationToken ct)
    {
        var zones = await _svc.GetLocationsAsync(stateCode, ct);

        return Ok(new { zones });
    }
}