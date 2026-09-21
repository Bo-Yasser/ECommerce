using ECommerce.Domain.Common;
using ECommerce.UseCases.Features.DeliveryMethods.Responses;
using MediatR;

namespace ECommerce.UseCases.Features.DeliveryMethods.Queries.GetDeliveryMethodById;

public sealed record GetDeliveryMethodByIdQuery(Guid Id) : IRequest<Result<DeliveryMethodResponse>>;