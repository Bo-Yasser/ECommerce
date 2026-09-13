using FluentValidation;
using Microsoft.Extensions.Options;

namespace ECommerce.UseCases.Common.Options;

public class MailSettingsValidator : AbstractValidator<MailSettings>, IValidateOptions<MailSettings>
{
    public MailSettingsValidator()
    {
        RuleFor(x => x.Host)
            .NotEmpty().WithMessage("SMTP Host is required.");

        RuleFor(x => x.Port)
            .GreaterThan(0).WithMessage("SMTP Port must be a valid port number.");

        RuleFor(x => x.SenderName)
            .NotEmpty().WithMessage("Sender Name is required.");

        RuleFor(x => x.SenderEmail)
            .NotEmpty().WithMessage("Sender Email is required.")
            .EmailAddress().WithMessage("Sender Email must be a valid email format.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("SMTP Password is required.");
    }

    public ValidateOptionsResult Validate(string? name, MailSettings options)
    {
        var validationResult = base.Validate(options);

        if (validationResult.IsValid)
            return ValidateOptionsResult.Success;

        var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
        return ValidateOptionsResult.Fail(errors);
    }
}