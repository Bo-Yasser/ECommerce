using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Errors;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using ECommerce.UseCases.Common.Interfaces;
using ECommerce.UseCases.Features.Basket.Enums;
using ECommerce.UseCases.Features.Basket.Responses;
using ECommerce.UseCases.Features.Basket.Specifications;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ECommerce.UseCases.Features.Basket.Commands.MergeBasket;

public sealed class MergeBasketCommandHandler(
    IBasketStore basketStore,
    IReadRepository<Product> productRepository,
    ICurrentUserService currentUserService,
    ILogger<MergeBasketCommandHandler> logger)
    : IRequestHandler<MergeBasketCommand, Result<MergeBasketResponse>>
{
    public async Task<Result<MergeBasketResponse>> Handle(MergeBasketCommand request, CancellationToken cancellationToken)
    {
        // get authenticated userId (buyerId)
        var userId = currentUserService.UserId;
        if (userId is null)
            return Result<MergeBasketResponse>.Failure(BasketErrors.AuthenticatedBuyerIdMissing);

        // get the original anonymous buyerId
        var anonymousId = currentUserService.GuestId;
        if (anonymousId is null)
            return Result<MergeBasketResponse>.Failure(BasketErrors.AnonymousBuyerRequired);

        // get the anonymousBasket with AnonymousBuyerId
        var anonymousBasket = await basketStore.GetAsync(anonymousId.Value, cancellationToken);
        
        // check if anonymousBasket existing and has items
        if (anonymousBasket is null || anonymousBasket.Items.Count == 0)
            return Result<MergeBasketResponse>.Failure(BasketErrors.AnonymousBasketNotFound);

        // get or create the authenticated user's basket
        var accountBasket = await basketStore.GetOrCreateAsync(userId.Value, cancellationToken);

        // get distinct productIds with items quantity
        var productIds = accountBasket.Items
            .Select(i => i.ProductId)
            .Concat(anonymousBasket.Items.Select(i => i.ProductId))
            .Distinct()
            .ToList();

        var quantities = productIds.ToDictionary(
                id => id,
                id => (
                    AccountQuantity: accountBasket .GetItemQuantity(id),
                    AnonymousQuantity: anonymousBasket.GetItemQuantity(id)
                    ));

        // merge the new basket with the anonymousBasket
        var mergeResult = accountBasket.MergeFrom(anonymousBasket);
        if (mergeResult.IsFailure)
            return Result<MergeBasketResponse>.Failure(mergeResult.Error!);

        // reconcile merged basket with the available stock
        var reconcileResult = await ReconcileMergedBasketAsync(
            accountBasket,
            productIds,
            quantities,
            cancellationToken);
        if (reconcileResult.IsFailure)
            return Result<MergeBasketResponse>.Failure(reconcileResult.Error!);

        var mergeAdjustmentsList = reconcileResult.Value;

        // save the new basket
        await basketStore.SaveAsync(accountBasket, cancellationToken);

        // delete old/anonymous Basket
        try
        {
            await basketStore.DeleteAsync(anonymousId.Value, cancellationToken);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Failed to delete guest basket {GuestBasketId} after successful merge.",
                anonymousId.Value);
        }

        return Result<MergeBasketResponse>.Success(new(
            BasketResponse.From(accountBasket, isGuest:false),
            mergeAdjustmentsList));
    }

    private async Task<Result<List<BasketMergeAdjustmentResponse>>> ReconcileMergedBasketAsync(
        Domain.Entities.Basket basket,
        List<Guid> relatedProductIds,
        Dictionary<Guid, (int AccountQuantity, int AnonymousQuantity)> preMergeQuantities,
        CancellationToken cancellationToken)
    {
        List<BasketMergeAdjustmentResponse> mergeAdjustmentsList = [];

        var products = await productRepository.ListAsync(
            new ProductsListByProductIdsWithStockForBasketSpecification(relatedProductIds),
            cancellationToken);

        var productsDictionary = products.ToDictionary(p => p.Id);

        foreach (var item in basket.Items.ToList())
        {
            var requestedQuantity =
                preMergeQuantities[item.ProductId].AccountQuantity +
                preMergeQuantities[item.ProductId].AnonymousQuantity;

            if (requestedQuantity > BasketItem.MaxQuantity)
            {
                mergeAdjustmentsList.Add(new(
                    item.ProductId,
                    requestedQuantity,
                    BasketItem.MaxQuantity,
                    BasketMergeAdjustmentReason.MaxQuantityExceeded));
            }

            if (!productsDictionary.TryGetValue(item.ProductId, out var product))
            {
                mergeAdjustmentsList.Add(new(
                    item.ProductId,
                    item.Quantity,
                    0,
                    BasketMergeAdjustmentReason.ProductNoLongerAvailable));

                basket.RemoveItem(item.ProductId);
                continue;
            }

            if (product.Stock is null)
            {
                mergeAdjustmentsList.Add(new(
                    item.ProductId,
                    item.Quantity,
                    0,
                    BasketMergeAdjustmentReason.StockUnavailable));

                basket.RemoveItem(item.ProductId);
                continue;
            }

            if (product.Stock.Quantity == 0)
            {
                mergeAdjustmentsList.Add(new(
                    item.ProductId,
                    item.Quantity,
                    0,
                    BasketMergeAdjustmentReason.OutOfStock));

                basket.RemoveItem(item.ProductId);
                continue;
            }

            var stockQuantity = product.Stock.Quantity;

            if (item.Quantity > stockQuantity)
            {
                mergeAdjustmentsList.Add(new(
                    item.ProductId,
                    item.Quantity,
                    stockQuantity,
                    BasketMergeAdjustmentReason.InsufficientStock));

                var adjustResult = basket.UpdateItemQuantity(item.ProductId, stockQuantity);

                if (adjustResult.IsFailure)
                    return Result<List<BasketMergeAdjustmentResponse>>.Failure(adjustResult.Error!);
            }
        }

        return Result<List<BasketMergeAdjustmentResponse>>.Success(mergeAdjustmentsList);
    }
}
