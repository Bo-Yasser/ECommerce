using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Errors;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using ECommerce.UseCases.Common.Interfaces;
using ECommerce.UseCases.Features.Basket.Responses;
using ECommerce.UseCases.Features.Basket.Specifications;
using MediatR;

namespace ECommerce.UseCases.Features.Basket.Commands.AddBasketItem;

public sealed class AddBasketItemCommandHandler(
    IBasketStore basketStore,
    IReadRepository<Product> productRepository,
    ICurrentUserService currentUserService)
    : IRequestHandler<AddBasketItemCommand, Result<BasketResponse>>
{
    public async Task<Result<BasketResponse>> Handle(AddBasketItemCommand request, CancellationToken cancellationToken)
    {
        var product = await productRepository.FirstOrDefaultAsync(
            new ProductWithStockForBasketSpecification(request.ProductId),
            cancellationToken);
        if (product is null)
            return Result<BasketResponse>.Failure(BasketErrors.ProductNotFound);

        if (product.Stock.Quantity == 0)
            return Result<BasketResponse>.Failure(BasketErrors.OutOfStock);

        var isGuest = !currentUserService.IsAuthenticated;
        var buyerId = currentUserService.BuyerId ?? Guid.NewGuid();

        var basket = await basketStore.GetOrCreateAsync(buyerId, cancellationToken);

        var totalQuantity = request.Quantity + basket.GetItemQuantity(product.Id);
        if (totalQuantity > product.Stock.Quantity)
            return Result<BasketResponse>.Failure(BasketErrors.InsufficientStock);

        var addResult = basket.AddItem(
            productId: product.Id,
            productName: product.Name,
            pictureUrl: product.PictureUrl,
            unitPrice: product.Price,
            quantity: request.Quantity);

        if (addResult.IsFailure)
            return Result<BasketResponse>.Failure(addResult.Error!);

        await basketStore.SaveAsync(basket, cancellationToken);
        return Result<BasketResponse>.Success(BasketResponse.From(basket, isGuest));
    }
}
