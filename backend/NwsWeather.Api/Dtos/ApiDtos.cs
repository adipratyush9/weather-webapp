namespace NwsWeather.Api.Dtos;

public sealed record StateDto(string Name, string Code);

public sealed record ZoneDto(string Id, string Name);

public sealed record DateWiseForecastDto(string Date, IReadOnlyList<ForecastPeriodDto> CurrentDateData);

public sealed record ForecastPeriodDto(
    string? Name,
    DateTimeOffset? StartTime,
    int? Temperature,
    string? TemperatureUnit,
    string? ShortForecast,
    string? DetailedForecast
);