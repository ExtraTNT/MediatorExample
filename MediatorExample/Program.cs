using System.Text;
using MediatorExample;
using MediatorExample.Application.Domain.Options;
using MediatorExample.Application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Swagger Auth
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "JWT Authorization header using the Bearer scheme.",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };
    c.AddSecurityDefinition("Bearer", securityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { securityScheme, new List<string>() }
    });
});

// Logger
builder.Host.UseSerilog();

// Auth
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = Constants.TokenIssuer,
        ValidAudience = Constants.TokenAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Constants.TokenSecret))
    };
});

// Additional Services
builder.Services.AddOptions<AppOptions>().Bind(builder.Configuration.GetSection(AppOptions.SectionKey));
builder.Services.AddSingleton<IUserRepository, UserRepositoryMock>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

// Build app
var app = builder.Build();

// Logger and Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.OAuthClientSecret(Constants.TokenSecret);
        c.OAuthUseBasicAuthenticationWithAccessCodeGrant();
    });
    Log.Logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.Console().WriteTo
        .File("logs/debug.txt", rollingInterval: RollingInterval.Infinite).CreateLogger();
    Log.Information("Application running in develop mode");
} else {
    Log.Logger = new LoggerConfiguration().MinimumLevel.Warning().WriteTo.Console().WriteTo
        .File("logs/prd.txt", rollingInterval: RollingInterval.Infinite).CreateLogger();
    Log.Information("Application running in production mode"); 
}
// Redirect, Auth, Mapping
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();