using FluentValidation;

namespace ECommerce.UseCases.Features.ProductBrands.Queries.GetBrandById;
public sealed class GetBrandByIdQueryValidator : AbstractValidator<GetBrandByIdQuery>
{
    public GetBrandByIdQueryValidator()
    {
        RuleFor(b => b.Id)
            .NotEmpty()
            .WithMessage("Brand Id is required");
    }
}
