using FluentValidation;

namespace ECommerce.UseCases.Features.Orders.Commands.ShipOrder;

public sealed class ShipOrderCommandValidator : AbstractValidator<ShipOrderCommand>
{
    public ShipOrderCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty().WithMessage("Order Id is required and cannot be empty.");

        RuleFor(x => x.RowVersion)
            .NotEmpty().WithMessage("Row Version is required and cannot be empty.");
    }
}
