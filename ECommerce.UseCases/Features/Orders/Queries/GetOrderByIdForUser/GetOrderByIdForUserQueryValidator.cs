using FluentValidation;

namespace ECommerce.UseCases.Features.Orders.Queries.GetOrderByIdForUser;

public sealed class GetOrderByIdForUserQueryValidator : AbstractValidator<GetOrderByIdForUserQuery>
{
    public GetOrderByIdForUserQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Order Id is required and cannot be empty.");
    }
}
