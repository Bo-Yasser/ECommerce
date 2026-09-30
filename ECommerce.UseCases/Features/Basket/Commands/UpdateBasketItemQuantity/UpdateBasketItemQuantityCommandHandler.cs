using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Errors;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using ECommerce.UseCases.Common.Interfaces;
using ECommerce.UseCases.Features.Basket.Responses;
using ECommerce.UseCases.Features.Basket.Specifications;
using MediatR;

namespace ECommerce.UseCases.Features.Basket.Commands.UpdateBasketItemQuantity;

public sealed class UpdateBasketItemQuantityCommandHandler(
    IBasketStore basketStore,
    IReadRepository<Product> productRepository,
    ICurrentUserService currentUserService)
    : IRequestHandler<UpdateBasketItemQuantityCommand, Result<BasketResponse>>
{
    public async Task<Result<BasketResponse>> Handle(UpdateBasketItemQuantityCommand request, CancellationToken cancellationToken)
    {
        var product = await productRepository.FirstOrDefaultAsync(
            new ProductWithStockForBasketSpecification(request.ProductId),
            cancellationToken);
        if (product is null)
            return Result<BasketResponse>.Failure(BasketErrors.ProductNotFound);

        var isGuest = !currentUserService.IsAuthenticated;
        var buyerId = currentUserService.BuyerId;
        if (buyerId is null)
            return Result<BasketResponse>.Failure(BasketErrors.AnonymousBuyerRequired);

        var basket = await basketStore.GetAsync(buyerId.Value, cancellationToken);
        if (basket is null)
            return Result<BasketResponse>.Failure(BasketErrors.BasketNotFound);

        if (product.Stock.Quantity == 0)
            return Result<BasketResponse>.Failure(BasketErrors.OutOfStock);

        if (request.Quantity > product.Stock.Quantity)
            return Result<BasketResponse>.Failure(BasketErrors.InsufficientStock);

        var updateResult = basket.UpdateItemQuantity(request.ProductId, request.Quantity);
        if (updateResult.IsFailure)
            return Result<BasketResponse>.Failure(updateResult.Error!);

        await basketStore.SaveAsync(basket, cancellationToken);
        return Result<BasketResponse>.Success(BasketResponse.From(basket, isGuest));
    }
}
