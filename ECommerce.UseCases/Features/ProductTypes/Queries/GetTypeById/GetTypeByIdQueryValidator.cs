using FluentValidation;

namespace ECommerce.UseCases.Features.ProductTypes.Queries.GetTypeById;

public sealed class GetTypeByIdQueryValidator : AbstractValidator<GetTypeByIdQuery>
{
    public GetTypeByIdQueryValidator()
    {
        RuleFor(t => t.Id)
            .NotEmpty()
            .WithMessage("Type Id is required");
    }
}
