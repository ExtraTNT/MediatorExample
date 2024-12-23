using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MediatorExample.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MediatorExample.Application.Attributes;

/// <summary>
/// Custom auth checker using password versions to not expose passwords
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class CustomAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        // Retrieve the scoped service from the service provider
        var userRepository = context.HttpContext.RequestServices.GetRequiredService<IUserRepository>();

        var token = context.HttpContext.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();
        var handler = new JwtSecurityTokenHandler();
        
        // Extract user-related information from claims
        if (handler.ReadToken(token) is not JwtSecurityToken jsonToken)
        {
            context.Result = new ForbidResult();
            return;
        }
        
        var passwordVersion = jsonToken.Claims.FirstOrDefault(claim => claim.Type == "passwdVersion")?.Value;
        var email = jsonToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email)?.Value;

        if (passwordVersion == null || email == null) {
           context.Result = new ForbidResult();
           return;
        }

        // Retrieve the user's expected PasswordVersion from the database using the scoped service
        var userPasswordVersion = userRepository.GetUserByEmail(email)!.PasswordVersion;
        
        // Compare the PasswordVersion from the token with the one from the database
        if (!string.Equals(passwordVersion, userPasswordVersion.ToString()))
        {
            context.Result = new ForbidResult();
        }
    }
}
