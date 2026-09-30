using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Errors;
using ECommerce.UseCases.Common.Interfaces;
using ECommerce.UseCases.Common.Options;
using MediatR;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;

namespace ECommerce.UseCases.Features.Auth.Commands.ResendOtp;

public sealed class ResendOtpCommandHandler(
    IPendingRegistrationStore pendingStore,
    IIdentityService identityService,
    IEmailService emailService,
    IOptions<EmailVerificationSettings> options) : IRequestHandler<ResendOtpCommand, Result>
{
    private readonly EmailVerificationSettings _settings = options.Value;
    public async Task<Result> Handle(ResendOtpCommand request, CancellationToken cancellationToken)
    {
        var email = request.Email.Trim();

        var pendingUser = await pendingStore.GetAsync(email, cancellationToken);
        if (pendingUser is null)
        {
            var isconfrimedUser = await identityService.IsEmailExists(email, cancellationToken);
            if (isconfrimedUser)
                return Result.Failure(IdentityErrors.EmailAlreadyConfirmed);

            return Result.Failure(IdentityErrors.UserNotFound with
            {
                Message = "No pending registration found. Please register again."
            });
        }

        if (pendingUser.FailedAttempts >= _settings.MaxFailedAttempts)
        {
            await pendingStore.RemoveAsync(pendingUser.Email, cancellationToken);
            return Result.Failure(AuthErrors.OtpRequestLimitExceeded);
        }

        var nextAllowedResendTime = pendingUser.LastOtpSendTime.AddMinutes(_settings.MaxMinutesToSendOtp);
        if (DateTimeOffset.UtcNow < nextAllowedResendTime)
        {
            return Result.Failure(AuthErrors.VerificationPending with
            {
                Message = $"Please wait at least {_settings.MaxMinutesToSendOtp} minute(s) before requesting a new OTP."
            });
        }

        var newOtpCode = RandomNumberGenerator.GetInt32(100000, 1000000).ToString();
        var subject = "E-Commerce Account Verification";
        var body = $"Welcome {pendingUser.FirstName}! Your NEW Verification code is: {newOtpCode}. It expires in {_settings.ExpirationMinutes} minutes.";

        var updatedPayload = pendingUser with
        {
            VerificationCode = newOtpCode,
            LastOtpSendTime = DateTimeOffset.UtcNow,
        };

        await pendingStore.SaveAsync(email, updatedPayload, cancellationToken);

        await emailService.SendEmailAsync(email, subject, body, cancellationToken);
        return Result.Success();
    }
}
