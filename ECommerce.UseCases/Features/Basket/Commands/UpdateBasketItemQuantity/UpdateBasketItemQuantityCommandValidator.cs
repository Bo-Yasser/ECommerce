using FluentValidation;

namespace ECommerce.UseCases.Features.Basket.Commands.UpdateBasketItemQuantity;

public class UpdateBasketItemQuantityCommandValidator : AbstractValidator<UpdateBasketItemQuantityCommand>
{
    public UpdateBasketItemQuantityCommandValidator()
    {
        RuleFor(command => command.ProductId)
            .NotEmpty()
            .WithMessage("Product Id is mandatory for quantity updates.");

        RuleFor(command => command.Quantity)
            .GreaterThan(0)
            .WithMessage("The updated quantity must be greater than zero. To remove an item, use the Remove endpoint.");

        RuleFor(command => command.BuyerId)
            .NotEmpty()
            .WithMessage("Buyer Id is required.");
    }
}