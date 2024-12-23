using MediatorExample.Application.Domain.Options;
using MediatR;
using Microsoft.Extensions.Options;

namespace MediatorExample.Application.Features.WeatherForecast.Today;

// some example stuff from ms
public class ForecastHandler : IRequestHandler<ForecastReadQuery, Domain.WeatherForecast[]>
{

    private readonly AppOptions _options;
    
    public ForecastHandler(IOptions<AppOptions> options) {
        _options = options.Value;
    }

    private static readonly string[] Summaries = new[]
     {
         "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
     };
    public Task<Domain.WeatherForecast[]> Handle(ForecastReadQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(Enumerable.Range(1, 5).Select(index => new Domain.WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)] + " " + _options.FunnyMessage
            })
            .ToArray());
    }
}