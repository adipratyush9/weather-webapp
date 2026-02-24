using NwsWeather.Api.Clients;
using NwsWeather.Api.Parsers;
using NwsWeather.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// // CORS for React Vite
// builder.Services.AddCors(options =>
// {
//     options.AddPolicy("dev", p =>
//         p.WithOrigins("http://localhost:5173")
//          .AllowAnyHeader()
//          .AllowAnyMethod());
// });
// CORS for React Vite (allow any localhost port)
builder.Services.AddCors(options =>
{
    options.AddPolicy("dev", policy =>
        policy
            .SetIsOriginAllowed(origin =>
            {
                if (string.IsNullOrWhiteSpace(origin)) return false;
                return Uri.TryCreate(origin, UriKind.Absolute, out var uri)
                       && (uri.Host == "localhost" || uri.Host == "127.0.0.1");
            })
            .AllowAnyHeader()
            .AllowAnyMethod());
});

// Typed HttpClient for NWS
builder.Services.AddHttpClient<INwsClient, NwsClient>(http =>
{
    http.BaseAddress = new Uri("https://api.weather.gov/");
    http.Timeout = TimeSpan.FromSeconds(30);

    //User-Agent details for NWS API
    http.DefaultRequestHeaders.UserAgent.ParseAdd("CanAmWeatherApp-AdiPratyush");
    http.DefaultRequestHeaders.Accept.ParseAdd("application/geo+json, application/json");
});

builder.Services.AddSingleton<IStateService, StateService>();
builder.Services.AddSingleton<CommonParser>();
builder.Services.AddScoped<IForecastService, ForecastService>();

var app = builder.Build();

app.UseCors("dev");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();