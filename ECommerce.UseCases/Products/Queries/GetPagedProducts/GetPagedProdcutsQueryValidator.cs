using FluentValidation;

namespace ECommerce.UseCases.Products.Queries.GetPagedProducts;

public class GetPagedProdcutsQueryValidator : AbstractValidator<GetPagedProductsQuery>
{
    public GetPagedProdcutsQueryValidator()
    {
        RuleFor(query => query.PageNumber)
            .GreaterThanOrEqualTo(1)
            .WithErrorCode("Products.PageNumber.Invalid")
            .WithMessage("Page number must be greater than or equal to 1.");

        RuleFor(query => query.PageSize)
            .InclusiveBetween(1, 1000)
            .WithErrorCode("Products.PageSize.Invalid")
            .WithMessage("Page size must be between 1 and 1000.");
    }

}
