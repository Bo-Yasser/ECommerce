using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Errors;
using ECommerce.UseCases.Common.Interfaces;
using ECommerce.UseCases.Common.Models;
using ECommerce.UseCases.Common.Options;
using MediatR;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;

namespace ECommerce.UseCases.Features.Auth.Commands.Register;

public sealed class RegisterCommandHandler(
    IIdentityService identityService,
    IPendingRegistrationStore pendingStore,
    IEmailService emailService,
    IOptions<EmailVerificationSettings> options) : IRequestHandler<RegisterCommand, Result>
{
    private readonly EmailVerificationSettings _settings = options.Value;
    public async Task<Result> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var email = request.Email.Trim();
        var firstName = request.FirstName.Trim();
        var lastName = request.LastName.Trim();

        if (await identityService.IsEmailExists(email, cancellationToken))
            return Result.Failure(IdentityErrors.EmailAlreadyConfirmed);

        var pendingPayload = await pendingStore.GetAsync(email, cancellationToken);
        if (pendingPayload is not null)
            return Result.Failure(AuthErrors.VerificationPending);

        var otpCode = RandomNumberGenerator.GetInt32(100000, 1000000).ToString();
        var hashedPassword = identityService.HashPassword(request.Password);

        var payload = new PendingRegistrationPayload(
            firstName,
            lastName,
            email,
            hashedPassword,
            otpCode,
            0,
            DateTimeOffset.UtcNow);

        // Save Payload in cache
        await pendingStore.SaveAsync(email, payload, cancellationToken);

        // Send Mail
        var subject = "E-Commerce Account Verification";
        var body = $"Welcome {firstName}! Your Verification code is: {otpCode}. It expires in {_settings.ExpirationMinutes} minutes.";
        await emailService.SendEmailAsync(email, subject, body, cancellationToken);
        
        return Result.Success();
        
    }
}