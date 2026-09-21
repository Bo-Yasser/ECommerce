using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Errors;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using ECommerce.UseCases.Features.DeliveryMethods.Responses;
using ECommerce.UseCases.Features.DeliveryMethods.Specifications;
using MediatR;

namespace ECommerce.UseCases.Features.DeliveryMethods.Queries.GetDeliveryMethodById;

public sealed class GetDeliveryMethodByIdQueryHandler(IReadRepository<DeliveryMethod> repository)
    : IRequestHandler<GetDeliveryMethodByIdQuery, Result<DeliveryMethodResponse>>
{
    public async Task<Result<DeliveryMethodResponse>> Handle(GetDeliveryMethodByIdQuery request, CancellationToken cancellationToken)
    {
        var deliveryMethod = await repository.FirstOrDefaultAsync(
            new DeliveryMethodByIdToResponseSpecification(request.Id),
            cancellationToken);

        if (deliveryMethod is null)
            return Result<DeliveryMethodResponse>.Failure(DeliveryMethodErrors.NotFound);

        return Result<DeliveryMethodResponse>.Success(deliveryMethod);
    }
}
