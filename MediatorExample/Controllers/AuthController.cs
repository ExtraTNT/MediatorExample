using MediatorExample.Application.Attributes;
using MediatorExample.Application.Domain;
using MediatorExample.Application.Features.Auth.ChangePasswd;
using MediatorExample.Application.Features.Auth.Login;
using MediatorExample.Application.Features.Auth.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MediatorExample.Controllers;
    
[ApiController]
[Route("[controller]")]
public class AuthController(IMediator mediator) : ControllerBase
{
    [HttpPost("register")]
    [Produces("application/json")]
    public async Task<ActionResult> Register([FromBody] User user)
    {
        var result = await mediator.Send(new RegisterQuery(user));
        return result? Ok() : Conflict();
    }
    
    [HttpPost("login")]
    [Produces("application/json")]
    public async Task<ActionResult> Login([FromBody] User user)
    {
        var result = await mediator.Send(new LoginQuery(user));
        if (result.Length == 0) return Unauthorized("Invalid email or password");
        return Ok(result);
    }
    
    [CustomAuthorize]
    [HttpPut("changepassword")]
    [Produces("application/json")]
    public async Task<ActionResult> ChangePasswd([FromBody] string newPassword)
    {
        var token = HttpContext.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();
        
        if (token == null) return Unauthorized();
        var result = await mediator.Send(new ChangePasswdQuery(token, newPassword));
        
        return result? Ok() : BadRequest();
    }
}