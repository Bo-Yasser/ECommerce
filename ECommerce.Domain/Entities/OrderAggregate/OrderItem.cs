using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Errors;

namespace ECommerce.Domain.Entities.OrderAggregate;

public sealed class OrderItem : BaseEntity
{
    private OrderItem() { }

    public Guid OrderId { get; private set; }
    public Order Order { get; private set; } = null!;
    public Guid ProductId { get; private set; }
    public ProductItemOrdered ItemOrdered { get; private set; } = null!;
    public int Quantity { get; private set; }

    public decimal LineTotal => ItemOrdered.UnitPrice * Quantity;

    internal static Result<OrderItem> Create(
        Guid id,
        Guid productId,
        ProductItemOrdered itemOrdered,
        int quantity)
    {
        if (id == Guid.Empty)
            return Result<OrderItem>.Failure(OrderErrors.OrderItemInvalidId);

        if (productId == Guid.Empty)
            return Result<OrderItem>.Failure(OrderErrors.ProductItemInvalidId);

        var orderItem = new OrderItem() { Id = id, ProductId = productId };

        var itemOrderedResult = orderItem.SetItemOrdered(itemOrdered);
        if (itemOrderedResult.IsFailure)
            return Result<OrderItem>.Failure(itemOrderedResult.Error!);

        var quantityResult = orderItem.SetQuantity(quantity);
        if (quantityResult.IsFailure)
            return Result<OrderItem>.Failure(quantityResult.Error!);

        return Result<OrderItem>.Success(orderItem);
    }

    private Result SetItemOrdered(ProductItemOrdered itemOrdered)
    {
        if (itemOrdered is null)
            return Result.Failure(OrderErrors.OrderItemProductRequired);

        ItemOrdered = itemOrdered;
        return Result.Success();
    }

    private Result SetQuantity(int quantity)
    {
        if (quantity <= 0)
            return Result.Failure(OrderErrors.OrderItemInvalidQuantity);

        Quantity = quantity;
        return Result.Success();
    }

    internal Result IncreaseQuantity(int quantity)
    {
        if (quantity <= 0)
            return Result.Failure(OrderErrors.OrderItemInvalidQuantity);

        Quantity += quantity;
        return Result.Success();
    }
    internal void SetOrderId(Guid orderId)
    {
        OrderId = orderId;
    }
}
