using ECommerce.Domain.Common;
using ECommerce.UseCases.Features.DeliveryMethods.Responses;
using MediatR;

namespace ECommerce.UseCases.Features.DeliveryMethods.Queries.GetDeliveryMethods;

public sealed record GetDeliveryMethodsQuery(
    bool AvailableOnly,
    string? Search) 
    : IRequest<Result<IReadOnlyList<DeliveryMethodResponse>>>;
