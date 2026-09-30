using FluentValidation;

namespace ECommerce.UseCases.Features.Orders.Queries.GetOrderById;

public sealed class GetOrderByIdQueryValidator : AbstractValidator<GetOrderByIdQuery>
{
    public GetOrderByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Order Id is required and cannot be empty.");
    }
}
