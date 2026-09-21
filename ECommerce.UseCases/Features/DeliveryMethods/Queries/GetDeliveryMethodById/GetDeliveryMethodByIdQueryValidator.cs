using FluentValidation;

namespace ECommerce.UseCases.Features.DeliveryMethods.Queries.GetDeliveryMethodById;

public sealed class GetDeliveryMethodByIdQueryValidator : AbstractValidator<GetDeliveryMethodByIdQuery>
{
    public GetDeliveryMethodByIdQueryValidator()
    {
        RuleFor(dm => dm.Id)
            .NotEmpty()
            .WithMessage("Delivery Method Id is required");
    }
}
