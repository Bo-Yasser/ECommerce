using FluentValidation;

namespace ECommerce.UseCases.Features.Basket.Commands.RemoveBasketItem;

public sealed class RemoveBasketItemCommandValidator : AbstractValidator<RemoveBasketItemCommand>
{
    public RemoveBasketItemCommandValidator()
    {
        RuleFor(command => command.ProductId)
            .NotEmpty()
            .WithMessage("Product Id is required.");
    }
}
