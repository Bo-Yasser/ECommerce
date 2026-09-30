using ECommerce.Domain.Common;
using MediatR;

namespace ECommerce.UseCases.Features.Orders.Commands.CancelOrder;

public sealed record CancelOrderCommand(Guid OrderId, byte[] RowVersion) : IRequest<Result>;