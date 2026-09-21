using ECommerce.Domain.Common;
using MediatR;

namespace ECommerce.UseCases.Features.DeliveryMethods.Commands.UpdateDeliveryMethod;

public sealed record UpdateDeliveryMethodCommand(
    Guid Id,
    string Name,
    decimal Price,
    string EstimatedDeliveryTime,
    string? Description,
    bool IsAvailable,
    int DisplayOrder) : IRequest<Result>;
