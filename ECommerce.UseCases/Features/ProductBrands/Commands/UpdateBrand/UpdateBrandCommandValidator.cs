using ECommerce.Domain.Entities;
using FluentValidation;

namespace ECommerce.UseCases.Features.ProductBrands.Commands.UpdateBrand;

public sealed class UpdateBrandCommandValidator : AbstractValidator<UpdateBrandCommand>
{
    public UpdateBrandCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Brand Id is required");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Brand name is required.")
            .MaximumLength(ProductBrand.MaxNameLength).WithMessage($"Brand name must not exceed {ProductBrand.MaxNameLength} characters.");
    }
}
