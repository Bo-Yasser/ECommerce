using ECommerce.Domain.Common;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using ECommerce.UseCases.Features.DeliveryMethods.Responses;
using ECommerce.UseCases.Features.DeliveryMethods.Specifications;
using MediatR;

namespace ECommerce.UseCases.Features.DeliveryMethods.Queries.GetDeliveryMethods;

public sealed class GetDeliveryMethodsQueryHandler(IReadRepository<DeliveryMethod> repository)
    : IRequestHandler<GetDeliveryMethodsQuery, Result<IReadOnlyList<DeliveryMethodResponse>>>
{
    public async Task<Result<IReadOnlyList<DeliveryMethodResponse>>> Handle(GetDeliveryMethodsQuery request, CancellationToken cancellationToken)
    {
        var deliveryMethods = await repository.ListAsync(
            new DeliveryMethodsListSpecification(request.AvailableOnly, request.Search),
            cancellationToken);

        return Result<IReadOnlyList<DeliveryMethodResponse>>.Success(deliveryMethods);
    }
}
