using NwsWeather.Api.Dtos;

namespace NwsWeather.Api.Clients;

public interface INwsClient
{
    Task<NwsFeatureCollection<NwsZoneProperties>> GetCountyZonesAsync(string stateCode, CancellationToken ct);
    Task<NwsZoneResponse> GetCountyZoneAsync(string zoneId, CancellationToken ct);
    Task<NwsPointResponse> GetPointAsync(string latLon, CancellationToken ct);
    Task<NwsForecastResponse> GetForecastByUrlAsync(string absoluteUrl, CancellationToken ct);
}