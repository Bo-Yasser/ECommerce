using ECommerce.Domain.Entities;
using FluentValidation;

namespace ECommerce.UseCases.Features.ProductBrands.Commands.CreateBrand;

public sealed class CreateBrandCommandValidator : AbstractValidator<CreateBrandCommand>
{
    public CreateBrandCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Brand name is required.")
            .MaximumLength(ProductBrand.MaxNameLength).WithMessage($"Brand name must not exceed {ProductBrand.MaxNameLength} characters.");
    }
}
