namespace ECommerce.UseCases.Common.Models;
public record AccessTokenResult(string AccessToken, DateTimeOffset ExpireAtUtc);
