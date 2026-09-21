using FluentValidation;

namespace ECommerce.UseCases.Features.ProductBrands.Commands.DeleteBrand;

public sealed class DeleteBrandCommandValidator : AbstractValidator<DeleteBrandCommand>
{
    public DeleteBrandCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Type Id is required");
    }
}
