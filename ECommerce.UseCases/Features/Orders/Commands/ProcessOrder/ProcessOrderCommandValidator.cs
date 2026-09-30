using FluentValidation;

namespace ECommerce.UseCases.Features.Orders.Commands.ProcessOrder;

public sealed class ProcessOrderCommandValidator : AbstractValidator<ProcessOrderCommand>
{
    public ProcessOrderCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty().WithMessage("Order Id is required and cannot be empty.");

        RuleFor(x => x.RowVersion)
            .NotEmpty().WithMessage("Row Version is required and cannot be empty.");
    }
}
