using ECommerce.Domain.Entities;
using FluentValidation;

namespace ECommerce.UseCases.Features.Products.Commands.UpdateProduct;

public sealed class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Product Id is required");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Product name is required.")
            .MaximumLength(Product.MaxNameLength).WithMessage($"Product name must not exceed {Product.MaxNameLength} characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Product description is required.")
            .MaximumLength(Product.MaxDescriptionLength).WithMessage($"Product description must not exceed {Product.MaxDescriptionLength} characters.");

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0).WithMessage("Price must be greater than or equal to 0.");

        RuleFor(x => x.PictureUrl)
            .NotEmpty().WithMessage("Picture URL is required.")
            .MaximumLength(Product.MaxPictureUrlLength).WithMessage($"Picture URL must not exceed {Product.MaxPictureUrlLength} characters.");

        RuleFor(x => x.ProductTypeId)
            .NotEmpty().WithMessage("Product type is required.");

        RuleFor(x => x.ProductBrandId)
            .NotEmpty().WithMessage("Product brand is required.");

        RuleFor(x => x.Sku)
            .NotEmpty().WithMessage("SKU is required.")
            .MaximumLength(50).WithMessage("SKU cannot exceed 50 characters.")
            .Matches("^[A-Z0-9_-]+$").WithMessage("SKU can only contain letters, numbers, dashes, and underscores.");
    }
}
