using System.Net.Http.Json;
using NwsWeather.Api.Dtos;

namespace NwsWeather.Api.Clients;

public sealed class NwsClient : INwsClient
{
    private readonly HttpClient _http;

    public NwsClient(HttpClient http) => _http = http;

    public async Task<NwsFeatureCollection<NwsZoneProperties>> GetCountyZonesAsync(string stateCode, CancellationToken ct)
    {
        var url = $"zones?area={Uri.EscapeDataString(stateCode)}&type=county";
        return (await _http.GetFromJsonAsync<NwsFeatureCollection<NwsZoneProperties>>(url, ct))
               ?? new NwsFeatureCollection<NwsZoneProperties>();
    }

    public async Task<NwsZoneResponse> GetCountyZoneAsync(string zoneId, CancellationToken ct)
    {
        var url = $"zones/county/{Uri.EscapeDataString(zoneId)}";
        return (await _http.GetFromJsonAsync<NwsZoneResponse>(url, ct)) ?? new NwsZoneResponse();
    }

    public async Task<NwsPointResponse> GetPointAsync(string latLon, CancellationToken ct)
    {
        var url = $"points/{Uri.EscapeDataString(latLon)}";
        return (await _http.GetFromJsonAsync<NwsPointResponse>(url, ct)) ?? new NwsPointResponse();
    }

    public async Task<NwsForecastResponse> GetForecastByUrlAsync(string absoluteUrl, CancellationToken ct)
    {
        using var req = new HttpRequestMessage(HttpMethod.Get, absoluteUrl);
        using var res = await _http.SendAsync(req, ct);
        res.EnsureSuccessStatusCode();

        return (await res.Content.ReadFromJsonAsync<NwsForecastResponse>(cancellationToken: ct))
               ?? new NwsForecastResponse();
    }
}