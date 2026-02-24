using Microsoft.AspNetCore.Mvc;
using NwsWeather.Api.Services;

namespace NwsWeather.Api.Controllers;

[ApiController]
[Route("api/states")]
public sealed class StatesController : ControllerBase
{
    private readonly IStateService _states;

    public StatesController(IStateService states)
    {
        _states = states;
    }

    [HttpGet]
    public IActionResult GetStates()
    {
        return Ok(_states.GetStates());
    }
}