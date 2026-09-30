using FluentValidation;

namespace ECommerce.UseCases.Features.Orders.Commands.DeliverOrder;

public sealed class DeliverOrderCommandValidator : AbstractValidator<DeliverOrderCommand>
{
    public DeliverOrderCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty().WithMessage("Order Id is required and cannot be empty.");

        RuleFor(x => x.RowVersion)
            .NotEmpty().WithMessage("Row Version is required and cannot be empty.");
    }
}
