using FluentValidation;

namespace ECommerce.UseCases.Features.Basket.Commands.MergeBasket;

public class MergeBasketCommandValidator : AbstractValidator<MergeBasketCommand>
{
    public MergeBasketCommandValidator()
    {
        RuleFor(command => command.AnonymousBuyerId)
            .NotEmpty()
            .WithMessage("The Anonymous Buyer Id is required to perform a basket migration.");

        RuleFor(command => command.BuyerId)
            .NotEmpty()
            .WithMessage("Destination Buyer Id is required.");
    }
}
