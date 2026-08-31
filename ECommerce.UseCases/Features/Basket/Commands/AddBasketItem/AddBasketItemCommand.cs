using ECommerce.Domain.Common;
using ECommerce.UseCases.Features.Basket.Responses;
using MediatR;

namespace ECommerce.UseCases.Features.Basket.Commands.AddBasketItem;

public sealed record AddBasketItemCommand(Guid BuyerId, Guid ProductId, int Quantity) : IRequest<Result<GetBasketResponse>>;