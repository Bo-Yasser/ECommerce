using ECommerce.Domain.Common;
using ECommerce.UseCases.Features.Orders.Responses;
using MediatR;

namespace ECommerce.UseCases.Features.Orders.Queries.GetOrderById;

public sealed record GetOrderByIdQuery(Guid Id) : IRequest<Result<OrderResponse>>;