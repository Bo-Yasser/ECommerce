using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Errors;
using ECommerce.Domain.Repositories;
using ECommerce.UseCases.Common.Interfaces;
using ECommerce.UseCases.Features.Basket.Responses;
using MediatR;

namespace ECommerce.UseCases.Features.Basket.Commands.MergeBasket;

public sealed class MergeBasketCommandHandler(
    IBasketStore basketStore,
    ICurrentUserService currentUserService)
    : IRequestHandler<MergeBasketCommand, Result<BasketResponse>>
{
    public async Task<Result<BasketResponse>> Handle(MergeBasketCommand request, CancellationToken cancellationToken)
    {
        // get authenticated userId (buyerId)
        var userId = currentUserService.UserId;
        if (userId is null)
            return Result<BasketResponse>.Failure(BasketErrors.AuthenticatedBuyerIdMissing);

        // get the original anonymous buyerId
        var anonymousId = currentUserService.GuestId;
        if (anonymousId is null)
            return Result<BasketResponse>.Failure(BasketErrors.AnonymousBuyerRequired);

        // get the anonymousBasket with AnonymousBuyerId
        var anonymousBasket = await basketStore.GetAsync(anonymousId.Value, cancellationToken);
        
        // check if anonymousBasket existing and has items
        if (anonymousBasket is null || anonymousBasket.Items.Count == 0)
            return Result<BasketResponse>.Failure(BasketErrors.AnonymousBasketNotFound);

        // create a new basket
        var basket = await basketStore.GetOrCreateAsync(userId.Value, cancellationToken);

        // merge the new basket with the anonymousBasket
        var mergeResult = basket.MergeFrom(anonymousBasket);
        if (mergeResult.IsFailure)
            return Result<BasketResponse>.Failure(mergeResult.Error!);

        // save the new basket
        await basketStore.SaveAsync(basket, cancellationToken);

        // delete old/anonymous Basket
        await basketStore.DeleteAsync(anonymousId.Value, cancellationToken);

        return Result<BasketResponse>.Success(BasketResponse.From(basket, isGuest:false));
    }
}
