using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Errors;
using ECommerce.Domain.Enums;

namespace ECommerce.Domain.Entities.OrderAggregate;

public sealed class Order : BaseEntity
{

    private readonly List<OrderItem> _items = [];
    public IReadOnlyList<OrderItem> Items => _items.AsReadOnly();
    public Guid UserId { get; private set; }
    public OrderStatus Status { get; private set; }
    public Guid DeliveryMethodId { get; private set; }
    public OrderDeliveryMethod DeliveryMethod { get; private set; } = null!;
    public ShippingAddress ShippingAddress { get; private set; } = null!;
    public decimal SubTotal { get; private set; } // total without shipping cost
    public decimal ShippingCost { get; private set; }
    public decimal Total { get; private set; } // total with shipping cost

    private Order() { }

    public static Result<Order> Create(
        Guid id,
        Guid userId,
        DeliveryMethod deliveryMethod,
        UserAddress userAddress)
    {
        var order = new Order() {  Status = OrderStatus.Pending };

        var idsResult = order.SetIdAndUserId(id, userId);
        if (idsResult.IsFailure)
            return Result<Order>.Failure(idsResult.Error!);

        var deliveryMethodResult = order.SetDeliveryMethod(deliveryMethod);
        if (deliveryMethodResult.IsFailure)
            return Result<Order>.Failure(deliveryMethodResult.Error!);

        var shippingAddressResult = order.SetShippingAddress(userAddress, userId);
        if (shippingAddressResult.IsFailure)
            return Result<Order>.Failure(shippingAddressResult.Error!);

        return Result<Order>.Success(order);
    }
    public Result AddItem(
        Guid productId, 
        ProductItemOrdered itemOrdered,
        int quantity)
    {
        if (Status != OrderStatus.Pending)
            return Result.Failure(OrderErrors.OnlyCanEditPendingOrder);

        if (itemOrdered is null)
            return Result.Failure(OrderErrors.OrderItemProductRequired);

        var existingItem = _items.FirstOrDefault(i => i.ProductId == productId);
        if(existingItem is not null)
    {
            var increaseResult = existingItem.IncreaseQuantity(quantity);
                if (increaseResult.IsFailure)
                    return Result.Failure(increaseResult.Error!);

            CalculateTotals();
            return Result.Success();
            }

        var orderItemResult = OrderItem.Create(Guid.NewGuid(), productId, itemOrdered, quantity);
            if (orderItemResult.IsFailure)
                return Result.Failure(orderItemResult.Error!);

        var item = orderItemResult.Value;

            item.SetOrderId(Id);
            _items.Add(item);

        CalculateTotals();

        return Result.Success();
    }

    public Result IncreaseItemQuantity(Guid productId, int quantity)
    {
        if (Status != OrderStatus.Pending)
            return Result.Failure(OrderErrors.OnlyCanEditPendingOrder);

        var item = _items.FirstOrDefault(i => i.ProductId == productId);
        if(item is null) return Result.Failure(OrderErrors.OrderItemNotFound);


        var increaseResult = item.IncreaseQuantity(quantity);
        if (increaseResult.IsFailure) return Result.Failure(increaseResult.Error!);

        CalculateTotals();
        return Result.Success();
    }


    public Result Cancel()
    {
        if (Status != OrderStatus.Pending)
            return Result.Failure(OrderErrors.CannotCancel);

        Status = OrderStatus.Cancelled;
        return Result.Success();
    }

    public Result Process()
    {
        if (Status != OrderStatus.Pending)
            return Result.Failure(OrderErrors.CannotProcess);

        Status = OrderStatus.Processing;
        return Result.Success();
    }

    public Result Deliver()
    {
        if(Status != OrderStatus.Shipped)
            return Result.Failure(OrderErrors.CannotDeliver);

        Status = OrderStatus.Delivered;
        return Result.Success();
    }

    public Result Ship()
    {
        if (Status != OrderStatus.Processing)
            return Result.Failure(OrderErrors.CannotShip);

        Status = OrderStatus.Shipped;
        return Result.Success();
    }

    private Result SetIdAndUserId(Guid id, Guid userId)
    {
        if (id == Guid.Empty)
            return Result.Failure(OrderErrors.InvalidId);

        if (userId == Guid.Empty)
            return Result.Failure(OrderErrors.InvalidUserId);

        Id = id;
        UserId = userId;
        return Result.Success();
    }

    private Result SetDeliveryMethod(DeliveryMethod deliveryMethod)
    {
        var deliveryMethodResult = OrderDeliveryMethod.FromDeliveryMethod(deliveryMethod);
        if (deliveryMethodResult.IsFailure)
            return Result.Failure(deliveryMethodResult.Error!);

        DeliveryMethodId = deliveryMethod.Id;
        DeliveryMethod = deliveryMethodResult.Value;
        ShippingCost = deliveryMethodResult.Value.DeliveryMethodPrice;

        return Result.Success();
    }

    private Result SetShippingAddress(UserAddress address, Guid userId)
    {
        if (address is null)
            return Result.Failure(OrderErrors.ShippingAddressRequired);

        if (address.UserId != userId)
            return Result.Failure(OrderErrors.ShippingAddressNotOwned);


        var shippingAddressResult = ShippingAddress.FromUserAddress(address);
        if (shippingAddressResult.IsFailure)
            return Result.Failure(shippingAddressResult.Error!);

        ShippingAddress = shippingAddressResult.Value;
        return Result.Success();
    }
    private void CalculateTotals()
    {
        SubTotal = _items.Sum(item => item.LineTotal);
        Total = SubTotal + ShippingCost;
    }

}