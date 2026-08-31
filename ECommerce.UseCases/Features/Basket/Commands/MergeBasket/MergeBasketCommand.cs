using ECommerce.Domain.Common;
using ECommerce.UseCases.Features.Basket.Responses;
using MediatR;

namespace ECommerce.UseCases.Features.Basket.Commands.MergeBasket;

public sealed record MergeBasketCommand(Guid BuyerId, Guid AnonymousBuyerId) : IRequest<Result<GetBasketResponse>>;
