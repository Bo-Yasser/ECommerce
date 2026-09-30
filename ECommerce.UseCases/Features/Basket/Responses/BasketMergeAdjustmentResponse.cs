using ECommerce.UseCases.Features.Basket.Enums;

namespace ECommerce.UseCases.Features.Basket.Responses;

public sealed record BasketMergeAdjustmentResponse(
    Guid ProductId,
    int RequestedQuantity,
    int FinalQuantity,
    BasketMergeAdjustmentReason Reason);