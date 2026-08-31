using ECommerce.Domain.Common;
using ECommerce.Domain.Repositories;
using ECommerce.UseCases.Features.Basket.Responses;
using MediatR;

namespace ECommerce.UseCases.Features.Basket.Commands.UpdateBasketItemQuantity;

public sealed class UpdateBasketItemQuantityCommandHandler(IBasketStore basketStore)
    : IRequestHandler<UpdateBasketItemQuantityCommand, Result<GetBasketResponse>>
{
    public async Task<Result<GetBasketResponse>> Handle(UpdateBasketItemQuantityCommand request, CancellationToken cancellationToken)
    {
        var basket = await basketStore.GetOrCreateAsync(request.BuyerId, cancellationToken);

        var updateResult = basket.UpdateItemQuantity(request.ProductId, request.Quantity);
        if (updateResult.IsFailure)
            return Result<GetBasketResponse>.Failure(updateResult.Error!);

        await basketStore.SaveAsync(basket, cancellationToken);
        return Result<GetBasketResponse>.Success(GetBasketResponse.From(basket));
    }
}
