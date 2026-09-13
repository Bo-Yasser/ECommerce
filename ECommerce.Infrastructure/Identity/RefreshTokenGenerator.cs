using ECommerce.UseCases.Common.Interfaces;
using System.Security.Cryptography;

namespace ECommerce.Infrastructure.Identity;

public sealed class RefreshTokenGenerator : IRefreshTokenGenerator
{
    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];

        using var rng = RandomNumberGenerator.Create();

        rng.GetBytes(randomNumber);

        return Convert.ToBase64String(randomNumber);
    }
}