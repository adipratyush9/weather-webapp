using NwsWeather.Api.Dtos;

namespace NwsWeather.Api.Services;

public interface IForecastService
{
    Task<IReadOnlyList<ZoneDto>> GetLocationsAsync(string stateCode, CancellationToken ct);
    Task<IReadOnlyList<DateWiseForecastDto>> GetForecastByZoneAsync(string zoneId, CancellationToken ct);
}