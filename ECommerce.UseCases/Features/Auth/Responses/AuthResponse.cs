namespace ECommerce.UseCases.Features.Auth.Responses;

public sealed record AuthResponse(string AccessToken, string RefreshToken, DateTimeOffset AccessTokenExpiration);
