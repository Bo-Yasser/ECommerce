using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Errors;
using ECommerce.Domain.Repositories;
using ECommerce.UseCases.Features.Basket.Responses;
using MediatR;

namespace ECommerce.UseCases.Features.Basket.Commands.MergeBasket;

public sealed class MergeBasketCommandHandler(IBasketStore basketStore)
    : IRequestHandler<MergeBasketCommand, Result<GetBasketResponse>>
{
    public async Task<Result<GetBasketResponse>> Handle(MergeBasketCommand request, CancellationToken cancellationToken)
    {

        // get the anonymousBasket with AnonymousBuyerId
        var anonymousBasket = await basketStore.GetAsync(request.AnonymousBuyerId, cancellationToken);
        
        // check if anonymousBasket existing and has items
        if (anonymousBasket is null || anonymousBasket.Items.Count == 0)
            return Result<GetBasketResponse>.Failure(BasketErrors.AnonymousBasketNotFound);

        // create a new basket
        var basket = await basketStore.GetOrCreateAsync(request.BuyerId, cancellationToken);

        // merge the new basket with the anonymousBasket
        var mergeResult = basket.MergeFrom(anonymousBasket);
        if (mergeResult.IsFailure)
            return Result<GetBasketResponse>.Failure(mergeResult.Error!);

        // save the new basket
        await basketStore.SaveAsync(basket, cancellationToken);

        // delete old/anonymous Basket
        await basketStore.DeleteAsync(request.AnonymousBuyerId, cancellationToken);

        return Result<GetBasketResponse>.Success(GetBasketResponse.From(basket));
    }
}
