using FluentValidation;

namespace ECommerce.UseCases.Features.Products.Queries.GetProductById;

public sealed class GetProductByIdQueryValidator : AbstractValidator<GetProductByIdQuery>
{
    public GetProductByIdQueryValidator()
    {
        RuleFor(p => p.Id)
            .NotEmpty()
            .WithMessage("Product Id is required");
    }
}
