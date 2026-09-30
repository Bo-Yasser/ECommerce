using FluentValidation;

namespace ECommerce.UseCases.Features.Orders.Queries.GetPagedOrders;

public sealed class GetPagedOrdersQueryValidator : AbstractValidator<GetPagedOrdersQuery>
{
    public GetPagedOrdersQueryValidator()
    {
        RuleFor(query => query.PageNumber)
            .GreaterThanOrEqualTo(1)
            .WithErrorCode("Orders.PageNumber.Invalid")
            .WithMessage("Page number must be greater than or equal to 1.");

        RuleFor(query => query.PageSize)
            .InclusiveBetween(1, 1000)
            .WithErrorCode("Orders.PageSize.Invalid")
            .WithMessage("Page size must be between 1 and 1000.");

        RuleFor(query => query.SortBy)
            .IsInEnum()
            .WithErrorCode("Orders.SortBy.Invalid")
            .WithMessage("Invalid order sort field.");
    }
}
