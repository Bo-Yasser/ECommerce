using ECommerce.Domain.Entities;
using FluentValidation;

namespace ECommerce.UseCases.Features.DeliveryMethods.Commands.UpdateDeliveryMethod;
public sealed class UpdateDeliveryMethodCommandValidator : AbstractValidator<UpdateDeliveryMethodCommand>
{
    public UpdateDeliveryMethodCommandValidator()
    {
        RuleFor(dm => dm.Id)
            .NotEmpty().WithMessage("Delivery method Id is required.");

        RuleFor(dm => dm.Name)
            .NotEmpty().WithMessage("Delivery method name is required.")
            .MaximumLength(DeliveryMethod.MaxNameLength)
            .WithMessage($"Delivery method name must not exceed {DeliveryMethod.MaxNameLength} characters.");

        RuleFor(dm => dm.Price)
            .GreaterThanOrEqualTo(0).WithMessage("Price must be greater than or equal to 0.");

        RuleFor(dm => dm.EstimatedDeliveryTime)
            .NotEmpty().WithMessage("Estimated delivery time is required.")
            .MaximumLength(DeliveryMethod.MaxDeliveryTimeLength)
            .WithMessage($"Estimated delivery time must not exceed {DeliveryMethod.MaxDeliveryTimeLength} characters.");

        RuleFor(dm => dm.Description)
            .MaximumLength(DeliveryMethod.MaxDescriptionLength)
            .WithMessage($"Description must not exceed {DeliveryMethod.MaxDescriptionLength} characters.")
            .When(dm => !string.IsNullOrWhiteSpace(dm.Description));

        RuleFor(dm => dm.DisplayOrder)
            .GreaterThanOrEqualTo(0).WithMessage("Display order must be a non-negative integer.");
    }
}
