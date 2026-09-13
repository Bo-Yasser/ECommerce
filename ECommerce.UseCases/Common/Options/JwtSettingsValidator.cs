using FluentValidation;
using Microsoft.Extensions.Options;

namespace ECommerce.UseCases.Common.Options;

public class JwtSettingsValidator : AbstractValidator<JwtSettings>, IValidateOptions<JwtSettings>
{
    public JwtSettingsValidator()
    {
        RuleFor(j => j.SecretKey)
            .NotEmpty()
            .WithMessage("SecretKey is required.");

        RuleFor(j => j.Issuer)
            .NotEmpty()
            .WithMessage("Issuer is required.");

        RuleFor(j => j.Audience)
            .NotEmpty()
            .WithMessage("Audience is required.");

        RuleFor(j => j.AccessTokenExpirationMinutes)
            .GreaterThanOrEqualTo(0)
            .WithMessage("AccessTokenExpirationMinutes must be greater than or equal to 0.");

        RuleFor(j => j.RefreshTokenExpirationDays)
            .GreaterThan(0)
            .WithMessage("RefreshTokenExpirationDays must be greater than 0.");
    }
    public ValidateOptionsResult Validate(string? name, JwtSettings options)
    {
        var validationResult = base.Validate(options);

        if (validationResult.IsValid)
        {
            return ValidateOptionsResult.Success;
        }

        var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
        return ValidateOptionsResult.Fail(errors);
    }
}
