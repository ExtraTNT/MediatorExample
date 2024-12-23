using MediatorExample.Application.Attributes;
using MediatorExample.Application.Features.WeatherForecast.Today;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MediatorExample.Controllers;

// some example stuff from ms
[ApiController]
[Route("api/[controller]")]
public class WeatherForecastController(IMediator mediator) : ControllerBase
{
    [CustomAuthorize]
    [HttpGet("today")]
    [Produces("application/json")]
    public async Task<ActionResult> Get()
    {
        var result = await mediator.Send(new ForecastReadQuery());
        return Ok(result);
    }
}