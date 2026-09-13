using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Errors;
using ECommerce.Domain.Repositories;
using ECommerce.UseCases.Common.Interfaces;
using ECommerce.UseCases.Features.Basket.Responses;
using MediatR;

namespace ECommerce.UseCases.Features.Basket.Commands.RemoveBasketItem;

public sealed class RemoveBasketItemCommandHandler(
    IBasketStore basketStore,
    ICurrentUserService currentUserService)
    : IRequestHandler<RemoveBasketItemCommand, Result<GetBasketResponse>>
{
    public async Task<Result<GetBasketResponse>> Handle(RemoveBasketItemCommand request, CancellationToken cancellationToken)
    {
        var isGuest = !currentUserService.IsAuthenticated;
        var buyerId = currentUserService.BuyerId;
        if (buyerId is null)
            return Result<GetBasketResponse>.Failure(BasketErrors.GuestBuyerIdRequired);

        var basket = await basketStore.GetOrCreateAsync(buyerId.Value, cancellationToken);
        if(basket is null)
            return Result<GetBasketResponse>.Failure(BasketErrors.BasketNotFound);

        var removeResult = basket.RemoveItem(request.ProductId);
        if (removeResult.IsFailure)
            return Result<GetBasketResponse>.Failure(removeResult.Error!);

        await basketStore.SaveAsync(basket, cancellationToken);
        return Result<GetBasketResponse>.Success(GetBasketResponse.From(basket, isGuest));
    }
}
