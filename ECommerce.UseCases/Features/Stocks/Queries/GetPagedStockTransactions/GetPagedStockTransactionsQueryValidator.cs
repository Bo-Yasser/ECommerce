using FluentValidation;

namespace ECommerce.UseCases.Features.Stocks.Queries.GetPagedStockTransactions;

public sealed class GetPagedStockTransactionsQueryValidator : AbstractValidator<GetPagedStockTransactionsQuery>
{
    public GetPagedStockTransactionsQueryValidator()
    {
        RuleFor(st => st.ProductId)
                .NotEmpty()
                .WithMessage("Product Id is required.");

        RuleFor(st => st.PageNumber)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page number must be greater than or equal to 1.");

        RuleFor(st => st.PageSize)
            .InclusiveBetween(1, 1000)
            .WithMessage("Page size must be between 1 and 1000.");

        RuleFor(query => query.SortBy)
            .IsInEnum()
            .WithErrorCode("StockTransactions.SortBy.Invalid")
            .WithMessage("Invalid Stock Transaction sort field.");
    }
}
