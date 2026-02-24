using System.Text.Json.Serialization;
using System.Text.Json;

namespace NwsWeather.Api.Dtos;

public sealed class NwsFeatureCollection<TProps>
{
    [JsonPropertyName("features")]
    public List<NwsFeature<TProps>> Features { get; set; } = [];
}

public sealed class NwsFeature<TProps>
{
    [JsonPropertyName("properties")]
    public TProps? Properties { get; set; }
}

public sealed class NwsZoneProperties
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

public sealed class NwsZoneResponse
{
    [JsonPropertyName("geometry")]
    public NwsGeometry? Geometry { get; set; }
}

public sealed class NwsGeometry
{
    [JsonPropertyName("coordinates")]
    public JsonElement Coordinates { get; set; }    
}

public sealed class NwsPointResponse
{
    [JsonPropertyName("properties")]
    public NwsPointProperties? Properties { get; set; }
}

public sealed class NwsPointProperties
{
    [JsonPropertyName("forecast")]
    public string? Forecast { get; set; }
}

public sealed class NwsForecastResponse
{
    [JsonPropertyName("properties")]
    public NwsForecastProperties? Properties { get; set; }
}

public sealed class NwsForecastProperties
{
    [JsonPropertyName("periods")]
    public List<NwsForecastPeriod> Periods { get; set; } = [];
}

public sealed class NwsForecastPeriod
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("startTime")]
    public DateTimeOffset? StartTime { get; set; }

    [JsonPropertyName("temperature")]
    public int? Temperature { get; set; }

    [JsonPropertyName("temperatureUnit")]
    public string? TemperatureUnit { get; set; }

    [JsonPropertyName("shortForecast")]
    public string? ShortForecast { get; set; }

    [JsonPropertyName("detailedForecast")]
    public string? DetailedForecast { get; set; }
}