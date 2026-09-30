using ECommerce.Domain.Common;
using MediatR;

namespace ECommerce.UseCases.Features.Orders.Commands.ShipOrder;

public sealed record ShipOrderCommand(Guid OrderId, byte[] RowVersion) : IRequest<Result>;
