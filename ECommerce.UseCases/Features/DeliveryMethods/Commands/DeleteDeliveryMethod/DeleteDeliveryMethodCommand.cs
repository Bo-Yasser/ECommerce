using ECommerce.Domain.Common;
using MediatR;

namespace ECommerce.UseCases.Features.DeliveryMethods.Commands.DeleteDeliveryMethod;

public sealed record DeleteDeliveryMethodCommand(Guid Id) : IRequest<Result>;
