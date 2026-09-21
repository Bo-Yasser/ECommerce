using ECommerce.Domain.Common;
using ECommerce.UseCases.Features.Basket.Responses;
using MediatR;

namespace ECommerce.UseCases.Features.Basket.Commands.RemoveBasketItem;

public sealed record RemoveBasketItemCommand(Guid ProductId) : IRequest<Result<BasketResponse>>;
