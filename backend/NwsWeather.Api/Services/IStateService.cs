using NwsWeather.Api.Dtos;

namespace NwsWeather.Api.Services;

public interface IStateService
{
    IReadOnlyList<StateDto> GetStates();
    bool IsValidStateCode(string code);
}