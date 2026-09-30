using FluentValidation;

namespace ECommerce.UseCases.Features.DeliveryMethods.Commands.DeleteDeliveryMethod;

public sealed class DeleteDeliveryMethodCommandValidator : AbstractValidator<DeleteDeliveryMethodCommand>
{
    public DeleteDeliveryMethodCommandValidator()
    {
        RuleFor(dm => dm.Id)
            .NotEmpty()
            .WithMessage("Delivery Method Id is required");

        RuleFor(x => x.RowVersion)
            .NotEmpty().WithMessage("Row Version is required and cannot be empty.");
    }
}
