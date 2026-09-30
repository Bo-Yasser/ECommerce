using ECommerce.Domain.Common;
using MediatR;

namespace ECommerce.UseCases.Features.Orders.Commands.DeliverOrder;

public sealed record DeliverOrderCommand(Guid OrderId, byte[] RowVersion) : IRequest<Result>;