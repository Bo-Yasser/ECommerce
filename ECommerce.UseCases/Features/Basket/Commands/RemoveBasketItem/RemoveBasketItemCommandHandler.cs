using ECommerce.Domain.Common;
using ECommerce.Domain.Repositories;
using ECommerce.UseCases.Features.Basket.Responses;
using MediatR;

namespace ECommerce.UseCases.Features.Basket.Commands.RemoveBasketItem;

public sealed class RemoveBasketItemCommandHandler(IBasketStore basketStore)
    : IRequestHandler<RemoveBasketItemCommand, Result<GetBasketResponse>>
{
    public async Task<Result<GetBasketResponse>> Handle(RemoveBasketItemCommand request, CancellationToken cancellationToken)
    {
        var basket = await basketStore.GetOrCreateAsync(request.BuyerId, cancellationToken);

        var removeResult = basket.RemoveItem(request.ProductId);
        if (removeResult.IsFailure)
            return Result<GetBasketResponse>.Failure(removeResult.Error!);

        await basketStore.SaveAsync(basket, cancellationToken);
        return Result<GetBasketResponse>.Success(GetBasketResponse.From(basket));
    }
}
