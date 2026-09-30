using FluentValidation;

namespace ECommerce.UseCases.Features.Orders.Commands.CancelOrder;

public sealed class CancelOrderCommandValidator : AbstractValidator<CancelOrderCommand>
{
    public CancelOrderCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty().WithMessage("Order Id is required and cannot be empty.");

        RuleFor(x => x.RowVersion)
            .NotEmpty().WithMessage("Row Version is required and cannot be empty.");
    }
}
