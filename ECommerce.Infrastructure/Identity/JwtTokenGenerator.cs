using ECommerce.UseCases.Common.Interfaces;
using ECommerce.UseCases.Common.Models;
using ECommerce.UseCases.Common.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ECommerce.Infrastructure.Identity;

public sealed class JwtTokenGenerator(IOptions<JwtSettings> settings) : IJwtTokenGenerator
{
    private readonly JwtSettings _settings = settings.Value;

    public AccessTokenResult GenerateToken(Guid userId, string email, string? displayName, IEnumerable<string> roles)
    {
        // Generate the expiration time for the token
        var expireAtUtc = DateTimeOffset.UtcNow.AddMinutes(_settings.AccessTokenExpirationMinutes);
        // Create the claims for the token
        var claims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(JwtRegisteredClaimNames.Exp, expireAtUtc.ToString()),
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        };

        // Add Custom Claims
        if (!string.IsNullOrEmpty(displayName))
        {
            claims.Add(new Claim("display_name", displayName));
        }

        // Add Roles
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        // Create the symmetric security key for signing the token
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SecretKey));

        // Create the signing credentials for the token
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // create the JWT token
        var token = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: claims,
            expires: expireAtUtc.UtcDateTime,
            signingCredentials: credentials
        );

        // Write the token to a string
        var written = new JwtSecurityTokenHandler().WriteToken(token);

        // Return the access token result
        return new AccessTokenResult(written, expireAtUtc);
    }

}
