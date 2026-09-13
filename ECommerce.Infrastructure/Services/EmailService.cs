using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using ECommerce.UseCases.Common.Interfaces;
using ECommerce.UseCases.Common.Options;

namespace ECommerce.Infrastructure.Services;

public sealed class EmailService(IOptions<MailSettings> options) : IEmailService
{
    private readonly MailSettings _settings = options.Value;

    public async Task SendEmailAsync(string to, string subject, string body, CancellationToken ct = default)
    {
        // 1. Message Construction via MimeKit
        var email = new MimeMessage();

        email.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderEmail));
        email.To.Add(MailboxAddress.Parse(to));
        email.Subject = subject;

        var builder = new BodyBuilder
        {
            HtmlBody = body // Injecting HTML payload directly
        };
        email.Body = builder.ToMessageBody();

        // 2. SMTP Connection via MailKit
        using var smtp = new SmtpClient();

        try
        {
            // Establish Connection with TLS Encryption
            await smtp.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls, ct);

            // Authenticate with the SMTP Server
            await smtp.AuthenticateAsync(_settings.SenderEmail, _settings.Password, ct);

            // Dispatch the Message
            await smtp.SendAsync(email, ct);
        }
        catch (Exception ex)
        {
            // Note: In a production scenario, log the exception using ILogger before throwing.
            throw new InvalidOperationException($"SMTP Dispatch Failed: {ex.Message}");
        }
        finally
        {
            // Guaranteed Resource Cleanup
            await smtp.DisconnectAsync(true, ct);
        }
    }
}
