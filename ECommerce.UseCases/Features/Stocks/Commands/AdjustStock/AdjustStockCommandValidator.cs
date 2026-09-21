using ECommerce.Domain.Entities.StockAggregate;
using FluentValidation;

namespace ECommerce.UseCases.Features.Stocks.Commands.AdjustStock;

internal class AdjustStockCommandValidator : AbstractValidator<AdjustStockCommand>
{
    public AdjustStockCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Product Id is required and cannot be empty.");

        RuleFor(x => x.NewQuantity)
            .GreaterThanOrEqualTo(0)
            .WithMessage("New quantity cannot be less than zero.");

        RuleFor(x => x.Notes)
            .MaximumLength(StockTransaction.MaxNotesLength)
            .WithMessage($"Notes cannot exceed {StockTransaction.MaxNotesLength} characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Notes));

        RuleFor(x => x.RowVersion)
            .NotEmpty().WithMessage("Row Version is required and cannot be empty.");

    }
}