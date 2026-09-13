namespace ECommerce.UseCases.Common.Options;

public sealed class MailSettings
{
    public const string SectionName = "MailSettings";

    public required string Host { get; init; }
    public required int Port { get; init; }
    public required string SenderName { get; init; }
    public required string SenderEmail { get; init; }
    public required string Password { get; init; }
}