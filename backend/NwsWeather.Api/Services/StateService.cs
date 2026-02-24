using NwsWeather.Api.Dtos;

namespace NwsWeather.Api.Services;

public sealed class StateService : IStateService
{
    private static readonly IReadOnlyDictionary<string, string> ByName =
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["Alabama"] = "AL",
            ["Alaska"] = "AK",
            ["American Samoa"] = "AS",
            ["Arizona"] = "AZ",
            ["Arkansas"] = "AR",
            ["California"] = "CA",
            ["Colorado"] = "CO",
            ["Connecticut"] = "CT",
            ["Delaware"] = "DE",
            ["District Of Columbia"] = "DC",
            ["Federated States Of Micronesia"] = "FM",
            ["Florida"] = "FL",
            ["Georgia"] = "GA",
            ["Guam"] = "GU",
            ["Hawaii"] = "HI",
            ["Idaho"] = "ID",
            ["Illinois"] = "IL",
            ["Indiana"] = "IN",
            ["Iowa"] = "IA",
            ["Kansas"] = "KS",
            ["Kentucky"] = "KY",
            ["Louisiana"] = "LA",
            ["Maine"] = "ME",
            ["Marshall Islands"] = "MH",
            ["Maryland"] = "MD",
            ["Massachusetts"] = "MA",
            ["Michigan"] = "MI",
            ["Minnesota"] = "MN",
            ["Mississippi"] = "MS",
            ["Missouri"] = "MO",
            ["Montana"] = "MT",
            ["Nebraska"] = "NE",
            ["Nevada"] = "NV",
            ["New Hampshire"] = "NH",
            ["New Jersey"] = "NJ",
            ["New Mexico"] = "NM",
            ["New York"] = "NY",
            ["North Carolina"] = "NC",
            ["North Dakota"] = "ND",
            ["Northern Mariana Islands"] = "MP",
            ["Ohio"] = "OH",
            ["Oklahoma"] = "OK",
            ["Oregon"] = "OR",
            ["Palau"] = "PW",
            ["Pennsylvania"] = "PA",
            ["Puerto Rico"] = "PR",
            ["Rhode Island"] = "RI",
            ["South Carolina"] = "SC",
            ["South Dakota"] = "SD",
            ["Tennessee"] = "TN",
            ["Texas"] = "TX",
            ["Utah"] = "UT",
            ["Vermont"] = "VT",
            ["Virgin Islands"] = "VI",
            ["Virginia"] = "VA",
            ["Washington"] = "WA",
            ["West Virginia"] = "WV",
            ["Wisconsin"] = "WI",
            ["Wyoming"] = "WY"
        };

    private static readonly HashSet<string> Codes =
        new(ByName.Values.Select(v => v.ToUpperInvariant()));

    public IReadOnlyList<StateDto> GetStates() =>
        ByName.OrderBy(kv => kv.Key)
              .Select(kv => new StateDto(kv.Key, kv.Value))
              .ToList();

    public bool IsValidStateCode(string code)
    {
        if (string.IsNullOrWhiteSpace(code)) return false;
        return Codes.Contains(code.Trim().ToUpperInvariant());
    }
}