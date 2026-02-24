using Microsoft.AspNetCore.Mvc;
using NwsWeather.Api.Services;

namespace NwsWeather.Api.Controllers;

[ApiController]
[Route("api/forecast")]
public sealed class ForecastController : ControllerBase
{
    private readonly IForecastService _svc;

    public ForecastController(IForecastService svc) => _svc = svc;

    [HttpGet("zone/{zoneId}")]
    public async Task<IActionResult> GetForecast(string zoneId, CancellationToken ct)
    {
        var data = await _svc.GetForecastByZoneAsync(zoneId, ct);
        return Ok(new { message = "Forecast Data", data });
    }
}