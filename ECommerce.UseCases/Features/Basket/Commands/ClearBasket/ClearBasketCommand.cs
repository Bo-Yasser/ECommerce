using ECommerce.Domain.Common;
using ECommerce.UseCases.Features.Basket.Responses;
using MediatR;

namespace ECommerce.UseCases.Features.Basket.Commands.ClearBasket;

public sealed record ClearBasketCommand(Guid BuyerId) : IRequest<Result<GetBasketResponse>>;
