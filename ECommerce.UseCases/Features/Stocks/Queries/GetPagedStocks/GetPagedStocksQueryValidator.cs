using FluentValidation;

namespace ECommerce.UseCases.Features.Stocks.Queries.GetPagedStocks;

public sealed class GetPagedStocksQueryValidator : AbstractValidator<GetPagedStocksQuery>
{
    public GetPagedStocksQueryValidator()
    {
        RuleFor(s => s.PageNumber)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page number must be greater than or equal to 1.");

        RuleFor(s => s.PageSize)
            .InclusiveBetween(1, 1000)
            .WithMessage("Page size must be between 1 and 1000.");
    }
}
