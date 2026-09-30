namespace ECommerce.UseCases.Features.Basket.Responses;

public sealed record MergeBasketResponse(
    BasketResponse Basket,
    IReadOnlyList<BasketMergeAdjustmentResponse> Adjustments);