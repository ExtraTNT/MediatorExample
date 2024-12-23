using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MediatorExample.Application.Domain;
using Microsoft.IdentityModel.Tokens;

namespace MediatorExample.Application.Extensions;

public static class UserExtensions
{
    /// <summary>
    /// Generates a token for the User
    /// </summary>
    /// <param name="user">User to generate token for</param>
    /// <returns>Token string of a JwtSecurityToken</returns>
    public static string GenerateToken(this User user){
        
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Constants.TokenSecret));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            Constants.TokenIssuer,
            Constants.TokenAudience,
            new List<Claim>
            {
                new (ClaimTypes.Email, user.Email),
                new ("passwdVersion", user.PasswordVersion.ToString())
                // Add other claims as needed
            },
            expires: DateTime.Now.AddHours(1),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    /// <summary>
    /// Gets the email of the user from a token
    /// </summary>
    /// <param name="token">The token to look for the email</param>
    /// <returns>The Email claim or null</returns>
    public static string? JwtGetUserEmail(this string token)
    {
        var handler = new JwtSecurityTokenHandler();

        // Read the token and create a SecurityToken
        // Extract user-related information from claims
        return handler.ReadToken(token) is JwtSecurityToken jsonToken ?
            jsonToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email)?.Value
            : null;
    }
}