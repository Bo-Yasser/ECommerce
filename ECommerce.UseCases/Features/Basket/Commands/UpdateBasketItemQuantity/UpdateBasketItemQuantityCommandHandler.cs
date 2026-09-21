using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Errors;
using ECommerce.Domain.Repositories;
using ECommerce.UseCases.Common.Interfaces;
using ECommerce.UseCases.Features.Basket.Responses;
using MediatR;

namespace ECommerce.UseCases.Features.Basket.Commands.UpdateBasketItemQuantity;

public sealed class UpdateBasketItemQuantityCommandHandler(
    IBasketStore basketStore,
    ICurrentUserService currentUserService)
    : IRequestHandler<UpdateBasketItemQuantityCommand, Result<BasketResponse>>
{
    public async Task<Result<BasketResponse>> Handle(UpdateBasketItemQuantityCommand request, CancellationToken cancellationToken)
    {
        var isGuest = !currentUserService.IsAuthenticated;
        var buyerId = currentUserService.BuyerId;
        if (buyerId is null)
            return Result<BasketResponse>.Failure(BasketErrors.AnonymousBuyerRequired);

        var basket = await basketStore.GetAsync(buyerId.Value, cancellationToken);
        if (basket is null)
            return Result<BasketResponse>.Failure(BasketErrors.BasketNotFound);

        var updateResult = basket.UpdateItemQuantity(request.ProductId, request.Quantity);
        if (updateResult.IsFailure)
            return Result<BasketResponse>.Failure(updateResult.Error!);

        await basketStore.SaveAsync(basket, cancellationToken);
        return Result<BasketResponse>.Success(BasketResponse.From(basket, isGuest));
    }
}
