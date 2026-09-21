using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Errors;

namespace ECommerce.Domain.Entities.OrderAggregate;
public sealed class OrderDeliveryMethod
{
    public const int MaxNameLength = 100;
    public const int MaxEstimatedTimeLength = 100;

    private OrderDeliveryMethod() { }

    public string DeliveryMethodName { get; private set; } = null!;
    public decimal DeliveryMethodPrice { get; private set; }
    public string DeliveryMethodEstimatedTime { get; private set; } = null!;

    public static Result<OrderDeliveryMethod> Create(
        string deliveryMethodName,
        decimal deliveryMethodPrice,
        string deliveryMethodEstimatedTime)
    {

        var orderDeliveryMethod = new OrderDeliveryMethod();

        var nameResult = orderDeliveryMethod.SetDeliveryMethodName(deliveryMethodName);
        if (nameResult.IsFailure)
            return Result<OrderDeliveryMethod>.Failure(nameResult.Error!);

        var priceResult = orderDeliveryMethod.SetDeliveryMethodPrice(deliveryMethodPrice);
        if (priceResult.IsFailure)
            return Result<OrderDeliveryMethod>.Failure(priceResult.Error!);

        var timeResult = orderDeliveryMethod.SetDeliveryMethodEstimatedTime(deliveryMethodEstimatedTime);
        if (timeResult.IsFailure)
            return Result<OrderDeliveryMethod>.Failure(timeResult.Error!);

        return Result<OrderDeliveryMethod>.Success(orderDeliveryMethod);
    }

    private Result SetDeliveryMethodName(string name)
    {
        var trimmed = name.Trim();
        if (string.IsNullOrWhiteSpace(trimmed))
            return Result.Failure(OrderErrors.OrderDeliveryMethodNameRequired);

        if (trimmed.Length > MaxNameLength)
            return Result.Failure(OrderErrors.OrderDeliveryMethodNameLengthExceeded);

        DeliveryMethodName = trimmed;
        return Result.Success();
    }

    private Result SetDeliveryMethodPrice(decimal price)
    {
        if (price < 0)
            return Result.Failure(OrderErrors.OrderDeliveryMethodNegativePrice);

        DeliveryMethodPrice = price;
        return Result.Success();
    }

    private Result SetDeliveryMethodEstimatedTime(string estimatedTime)
    {
        var trimmed = estimatedTime.Trim();
        if (string.IsNullOrWhiteSpace(trimmed))
            return Result.Failure(OrderErrors.OrderDeliveryMethodTimeRequired);

        if (trimmed.Length > MaxEstimatedTimeLength)
            return Result.Failure(OrderErrors.OrderDeliveryMethodTimeLengthExceeded);

        DeliveryMethodEstimatedTime = trimmed;
        return Result.Success();
    }

    public static Result<OrderDeliveryMethod> FromDeliveryMethod(DeliveryMethod deliveryMethod)
    {
        if (deliveryMethod is null)
            return Result<OrderDeliveryMethod>.Failure(OrderErrors.OrderDeliveryMethodRequired);

        return Create(
            deliveryMethod.Name,
            deliveryMethod.Price,
            deliveryMethod.EstimatedDeliveryTime);
    }
}