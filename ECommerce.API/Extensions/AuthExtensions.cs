using ECommerce.UseCases.Common.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ECommerce.API.Extensions;

public static class AuthExtensions
{
    public static IServiceCollection AddAuthenticationAndAuthorization(this IServiceCollection services, IConfiguration config)
    {
        var jwtSettings = config.GetSection("Jwt").Get<JwtSettings>();
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true, // turn on issuer validation
                ValidIssuer = jwtSettings!.Issuer, // valid issuer value from configuration

                ValidateAudience = true, // turn on audience validation
                ValidAudience = jwtSettings!.Audience, // valid audience value from configuration

                ValidateIssuerSigningKey = true, // turn on signing key validation
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings!.SecretKey)), // signing key from configuration

                ValidateLifetime = true, // turn on lifetime validation, for validate the expiration and not before values in the token
                ClockSkew = TimeSpan.Zero // set clock skew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)

            };
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    if (context.Request.Cookies.TryGetValue("X-Access-Token", out var token))
                    {
                        context.Token = token;
                    }
                    return Task.CompletedTask;
                }
            };
        });

        services.AddAuthorization();

        return services;
    }
}
