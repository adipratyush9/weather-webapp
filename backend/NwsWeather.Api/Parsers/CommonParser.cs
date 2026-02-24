using NwsWeather.Api.Dtos;
using System.Text.Json;

namespace NwsWeather.Api.Parsers;

public sealed class CommonParser
{
    public string GetAverageLatLon(List<List<double>> ring)
    {
        // ring contains [lon,lat] pairs
        double lon = 0, lat = 0;
        int n = 0;

        foreach (var pair in ring)
        {
            if (pair.Count < 2) continue;
            lon += pair[0];
            lat += pair[1];
            n++;
        }

        if (n == 0) throw new InvalidOperationException("No valid coordinates.");
        lon /= n;
        lat /= n;

        return $"{lat},{lon}";
    }


    public List<List<double>> ExtractFirstRing(JsonElement coordinates)
    {


        if (coordinates.ValueKind != JsonValueKind.Array)
            throw new InvalidOperationException("Invalid geometry.coordinates (not an array).");

        var first = coordinates[0];
        if (first.ValueKind != JsonValueKind.Array)
            throw new InvalidOperationException("Invalid geometry.coordinates[0].");

        var maybeRing = first[0];

        if (maybeRing.ValueKind == JsonValueKind.Array && maybeRing.GetArrayLength() == 2 && maybeRing[0].ValueKind == JsonValueKind.Number)
        {
            return ReadRing(first);
        }

        return ReadRing(first[0]);
    }

    private static List<List<double>> ReadRing(JsonElement ringEl)
    {
        var ring = new List<List<double>>();

        foreach (var point in ringEl.EnumerateArray())
        {
            if (point.ValueKind != JsonValueKind.Array || point.GetArrayLength() < 2) continue;

            var lon = point[0].GetDouble();
            var lat = point[1].GetDouble();
            ring.Add(new List<double> { lon, lat });
        }

        if (ring.Count == 0)
            throw new InvalidOperationException("No valid coordinates in ring.");

        return ring;
    }

    public IReadOnlyList<DateWiseForecastDto> GroupByDate(IEnumerable<ForecastPeriodDto> periods)
    {
        var result = new List<DateWiseForecastDto>();

        string? currentDate = null;
        var bucket = new List<ForecastPeriodDto>();

        foreach (var p in periods.Where(x => x.StartTime != null).OrderBy(x => x.StartTime))
        {
            var dateStr = p.StartTime!.Value.LocalDateTime.ToShortDateString();

            if (currentDate == null || currentDate != dateStr)
            {
                if (currentDate != null)
                    result.Add(new DateWiseForecastDto(currentDate, bucket));

                currentDate = dateStr;
                bucket = [p];
            }
            else
            {
                bucket.Add(p);
            }
        }

        if (currentDate != null)
            result.Add(new DateWiseForecastDto(currentDate, bucket));

        return result;
    }
}