using FluentValidation;
using Microsoft.Extensions.Options;

namespace ECommerce.UseCases.Common.Options;

public class CacheEntryPolicyValidator : AbstractValidator<CacheEntryPolicy>, IValidateOptions<CacheEntryPolicy>
{
    public CacheEntryPolicyValidator()
    {
        RuleFor(c => c.AbsoluteExpirationDays)
            .GreaterThan(0);
        
        RuleFor(c => c.SlidingExpirationDays)
            .GreaterThan(0)
            .LessThanOrEqualTo(c => c.AbsoluteExpirationDays);

        RuleFor(c => c.LocalCacheExpirationMinutes)
            .GreaterThan(0);

        RuleFor(c => c.SlidingRefreshThresholdMinutes)
            .GreaterThan(0)
            .LessThanOrEqualTo(c => (int)TimeSpan.FromDays(c.SlidingExpirationDays).TotalMinutes)
            .WithMessage("Sliding refresh expiration cannot exceed sliding expiration.");
    }

    public ValidateOptionsResult Validate(string? name, CacheEntryPolicy options)
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
