using FluentValidation;

namespace ECommerce.UseCases.Features.Basket.Commands.AddBasketItem;

public class AddBasketItemCommandValidator : AbstractValidator<AddBasketItemCommand>
{
    public AddBasketItemCommandValidator()
    {
        RuleFor(command => command.ProductId)
            .NotEmpty()
            .WithMessage("Product Id is mandatory and cannot be empty.");


        RuleFor(command => command.Quantity)
            .GreaterThan(0)
            .WithMessage("The quantity of the item to add must be greater than zero.");


        RuleFor(command => command.BuyerId)
            .NotEmpty()
            .WithMessage("Buyer Id is required.");
    }
}