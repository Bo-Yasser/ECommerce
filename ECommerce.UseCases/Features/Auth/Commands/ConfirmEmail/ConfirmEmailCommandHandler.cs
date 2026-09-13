using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Errors;
using ECommerce.UseCases.Common.Interfaces;
using ECommerce.UseCases.Common.Options;
using MediatR;
using Microsoft.Extensions.Options;

namespace ECommerce.UseCases.Features.Auth.Commands.ConfirmEmail;

public sealed class ConfirmEmailCommandHandler(
    IIdentityService identityService,
    IPendingRegistrationStore pendingStore,
    IOptions<EmailVerificationSettings> options) : IRequestHandler<ConfirmEmailCommand, Result>
{
    private readonly EmailVerificationSettings _settings = options.Value;
    public async Task<Result> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var email = request.Email.Trim();
        var code = request.VerificationCode.Trim();

        var payload = await pendingStore.ValidateAndConsumeAsync(email, code, cancellationToken);
        if (payload is null)
        {
            if (await identityService.IsEmailExists(email, cancellationToken))
                return Result.Failure(IdentityErrors.EmailAlreadyConfirmed);

            return Result.Failure(IdentityErrors.UserNotFound with
            {
                Message = "No pending registration found for this email. It may have expired. Please register again."
            });
        }

        if(payload.VerificationCode != code)
        {
            if (payload.FailedAttempts >= _settings.MaxFailedAttempts)
            {
                await pendingStore.RemoveAsync(payload.Email, cancellationToken);
                return Result.Failure(AuthErrors.OtpRequestLimitExceeded);
            }

            var remainAttempts = _settings.MaxFailedAttempts - payload.FailedAttempts;
            return Result.Failure(AuthErrors.InvalidVerificationCode with
            {
                Message = $"Invalid verification code. You have {remainAttempts} attempt(s) remaining."
            });
        }

        var fullName = $"{payload.FirstName} {payload.LastName}".Trim();

        var result = await identityService.CreateVerifiedUserAsync(
            email: payload.Email,
            preHashedPassword: payload.PasswordHash,
            displayName: fullName,
            cancellationToken: cancellationToken);

        if (result.IsFailure)
            return Result.Failure(result.Error!);

        return Result.Success();
    }
}
