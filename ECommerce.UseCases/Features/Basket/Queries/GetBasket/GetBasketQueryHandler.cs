using ECommerce.Domain.Common;
using ECommerce.Domain.Repositories;
using ECommerce.UseCases.Common.Interfaces;
using ECommerce.UseCases.Features.Basket.Responses;
using MediatR;

namespace ECommerce.UseCases.Features.Basket.Queries.GetBasket;

public sealed class GetBasketQueryHandler(
    IBasketStore basketStore,
    ICurrentUserService currentUserService) : IRequestHandler<GetBasketQuery, Result<BasketResponse>>
{
    public async Task<Result<BasketResponse>> Handle(GetBasketQuery request, CancellationToken cancellationToken)
    {
        var isGuest = !currentUserService.IsAuthenticated;
        var buyerId = currentUserService.BuyerId ?? Guid.NewGuid();

        var basket = await basketStore.GetOrCreateAsync(buyerId, cancellationToken);

        return Result<BasketResponse>.Success(BasketResponse.From(basket, isGuest));
    }
}