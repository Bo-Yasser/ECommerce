using ECommerce.Domain.Common;
using ECommerce.UseCases.Features.Orders.Responses;
using MediatR;

namespace ECommerce.UseCases.Features.Orders.Commands.CreateOrder;

public sealed record CreateOrderCommand(
    Guid ShippingAddressId,
    Guid DeliveryMethodId) : IRequest<Result<OrderResponse>>;
