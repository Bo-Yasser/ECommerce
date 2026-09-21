using FluentValidation;

namespace ECommerce.UseCases.Features.Stocks.Queries.GetStockByProductId;

public sealed class GetStockByProductIdQueryValidator : AbstractValidator<GetStockByProductIdQuery>
{
    public GetStockByProductIdQueryValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Product Id is required and cannot be empty.");
    }
}
