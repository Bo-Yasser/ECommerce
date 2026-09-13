using FluentValidation;
using Microsoft.Extensions.Options;


namespace ECommerce.UseCases.Common.Options;

public class EmailVerificationSettingsValidator : AbstractValidator<EmailVerificationSettings>, IValidateOptions<EmailVerificationSettings>
{
    public EmailVerificationSettingsValidator()
    {
        RuleFor(x => x.CodeLength)
            .InclusiveBetween(4, 10)
            .WithMessage("Verification code length must be between 4 and 10 characters.");

        RuleFor(x => x.ExpirationMinutes)
            .InclusiveBetween(1, 60)
            .WithMessage("Expiration must be between 1 and 60 minutes for security reasons.");

        RuleFor(x => x.MaxFailedAttempts)
            .GreaterThan(0);

        RuleFor(x => x.MaxTimeToSendOtp)
            .GreaterThan(0);
    }

    public ValidateOptionsResult Validate(string? name, EmailVerificationSettings options)
    {
        var validationResult = base.Validate(options);

        if (validationResult.IsValid)
            return ValidateOptionsResult.Success;

        var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
        return ValidateOptionsResult.Fail(errors);
    }
}
