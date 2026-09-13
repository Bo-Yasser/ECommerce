namespace ECommerce.UseCases.Common.Options;

public sealed class JwtSettings
{
    public const string SectionName = "Jwt";
    public string SecretKey { get; set; } = null!;
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
    public int AccessTokenExpirationMinutes { get; set; } = 15;
    public int RefreshTokenExpirationDays { get; set; } = 7;
    public int MaxSessionsLimit { get; set; } = 5;

}