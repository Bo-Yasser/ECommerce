using ECommerce.Domain.Common;
using ECommerce.UseCases.Features.Orders.Responses;
using MediatR;

namespace ECommerce.UseCases.Features.Orders.Queries.GetOrderByIdForUser;

public sealed record GetOrderByIdForUserQuery(Guid Id) : IRequest<Result<OrderResponse>>;
