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
        UserAddress userAddress,
        IReadOnlyList<OrderItemPayload> items)
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

        var itemsResult = order.SetItems(items);
        if (itemsResult.IsFailure)
            return Result<Order>.Failure(itemsResult.Error!);

        return Result<Order>.Success(order);
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
        if(shippingAddressResult.IsFailure)
            return Result.Failure(shippingAddressResult.Error!);

        ShippingAddress = shippingAddressResult.Value;
        return Result.Success();
    }
    private void CalculateTotals()
    {
        SubTotal = _items.Sum(item => item.LineTotal);
        Total = SubTotal + ShippingCost;
    }
    private Result SetItems(IReadOnlyList<OrderItemPayload> items)
    {
        if (items is null || items.Count == 0)
            return Result.Failure(OrderErrors.EmptyOrderItems);

        var itemDictionary = new Dictionary<Guid, OrderItem>();

        foreach (var payload in items)
        {
            if (itemDictionary.TryGetValue(payload.ProductId, out var existingItem))
            {
                var increaseResult = existingItem.IncreaseQuantity(payload.Quantity);
                if (increaseResult.IsFailure)
                    return Result.Failure(increaseResult.Error!);

                continue;
            }

            var orderItemResult = OrderItem.Create(Guid.NewGuid(), payload.ProductId, payload.Product, payload.Quantity);
            if (orderItemResult.IsFailure)
                return Result.Failure(orderItemResult.Error!);

            itemDictionary.Add(payload.ProductId, orderItemResult.Value);
        }

        foreach (var item in itemDictionary.Values)
        {
            item.SetOrderId(Id);
            _items.Add(item);
        }

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
            return Result.Failure(OrderErrors.InvalidPaymentState);

        Status = OrderStatus.Processing;
        return Result.Success();
    }

}