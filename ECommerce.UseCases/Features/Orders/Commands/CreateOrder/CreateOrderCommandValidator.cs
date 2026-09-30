using FluentValidation;

namespace ECommerce.UseCases.Features.Orders.Commands.CreateOrder;

public sealed class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.ShippingAddressId)
            .NotEmpty()
            .WithMessage("Shipping address is required.");

        RuleFor(x => x.DeliveryMethodId)
            .NotEmpty()
            .WithMessage("Delivery method is required.");
    }
}