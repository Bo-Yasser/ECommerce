using ECommerce.Domain.Common;
using MediatR;

namespace ECommerce.UseCases.Features.Orders.Commands.ProcessOrder;

public sealed record ProcessOrderCommand(Guid OrderId, byte[] RowVersion) : IRequest<Result>;
