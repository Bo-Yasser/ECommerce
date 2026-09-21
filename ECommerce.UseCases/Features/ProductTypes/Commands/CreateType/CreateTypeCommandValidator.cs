using ECommerce.Domain.Entities;
using FluentValidation;

namespace ECommerce.UseCases.Features.ProductTypes.Commands.CreateType;

public sealed class CreateTypeCommandValidator : AbstractValidator<CreateTypeCommand>
{
    public CreateTypeCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Type name is required.")
            .MaximumLength(ProductType.MaxNameLength).WithMessage($"Type name must not exceed {ProductType.MaxNameLength} characters.");

    }
}
