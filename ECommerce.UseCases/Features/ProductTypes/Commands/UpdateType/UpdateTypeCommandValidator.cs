using ECommerce.Domain.Entities;
using FluentValidation;

namespace ECommerce.UseCases.Features.ProductTypes.Commands.UpdateType;

public sealed class UpdateTypeCommandValidator : AbstractValidator<UpdateTypeCommand>
{
    public UpdateTypeCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Type Id is required");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Type name is required.")
            .MaximumLength(ProductType.MaxNameLength).WithMessage($"Type name must not exceed {ProductType.MaxNameLength} characters.");

    }
}
