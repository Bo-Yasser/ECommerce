using ECommerce.Domain.Common;
using ECommerce.UseCases.Features.Basket.Responses;
using MediatR;

namespace ECommerce.UseCases.Features.Basket.Commands.UpdateBasketItemQuantity;

public sealed record UpdateBasketItemQuantityCommand(Guid ProductId, int Quantity) : IRequest<Result<BasketResponse>>;
