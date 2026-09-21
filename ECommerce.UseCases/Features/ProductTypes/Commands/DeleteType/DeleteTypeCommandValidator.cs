using FluentValidation;

namespace ECommerce.UseCases.Features.ProductTypes.Commands.DeleteType;

public sealed class DeleteTypeCommandValidator : AbstractValidator<DeleteTypeCommand>
{
    public DeleteTypeCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Type Id is required");
    }
}
