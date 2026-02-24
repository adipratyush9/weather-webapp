using NwsWeather.Api.Clients;
using NwsWeather.Api.Dtos;
using NwsWeather.Api.Parsers;

namespace NwsWeather.Api.Services;

public sealed class ForecastService : IForecastService
{
    private readonly INwsClient _nws;
    private readonly IStateService _states;
    private readonly CommonParser _parser;

    public ForecastService(INwsClient nws, IStateService states, CommonParser parser)
    {
        _nws = nws;
        _states = states;
        _parser = parser;
    }

    public async Task<IReadOnlyList<ZoneDto>> GetLocationsAsync(string stateCode, CancellationToken ct)
    {
        if (!_states.IsValidStateCode(stateCode))
            throw new ArgumentException("State is wrong.");

        var code = stateCode.Trim().ToUpperInvariant();
        var zones = await _nws.GetCountyZonesAsync(code, ct);

        return zones.Features
            .Select(f => f.Properties)
            .Where(p => p?.Id != null && p.Name != null)
            .Select(p => new ZoneDto(p!.Id!, p!.Name!))
            .OrderBy(z => z.Name)
            .ToList();
    }

    public async Task<IReadOnlyList<DateWiseForecastDto>> GetForecastByZoneAsync(string zoneId, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(zoneId))
            throw new ArgumentException("Zone id is required.");

        var zone = await _nws.GetCountyZoneAsync(zoneId.Trim(), ct);
        
        if (zone.Geometry is null)
            throw new InvalidOperationException("Zone geometry not found.");

        var ring = _parser.ExtractFirstRing(zone.Geometry.Coordinates);
        var latLon = _parser.GetAverageLatLon(ring);

        var point = await _nws.GetPointAsync(latLon, ct);
        var forecastUrl = point.Properties?.Forecast;

        if (string.IsNullOrWhiteSpace(forecastUrl))
            throw new InvalidOperationException("Forecast URL not found from points endpoint.");

        var forecast = await _nws.GetForecastByUrlAsync(forecastUrl, ct);

        var periods = (forecast.Properties?.Periods ?? [])
            .Select(p => new ForecastPeriodDto(
                p.Name,
                p.StartTime,
                p.Temperature,
                p.TemperatureUnit,
                p.ShortForecast,
                p.DetailedForecast
            ))
            .ToList();

        return _parser.GroupByDate(periods);
    }
}