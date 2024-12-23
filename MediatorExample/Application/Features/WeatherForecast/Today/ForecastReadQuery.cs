using MediatR;

namespace MediatorExample.Application.Features.WeatherForecast.Today;

// some example stuff from ms
public record ForecastReadQuery: IRequest<Domain.WeatherForecast[]>;