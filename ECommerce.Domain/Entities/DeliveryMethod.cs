using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Errors;

namespace ECommerce.Domain.Entities;

public sealed class DeliveryMethod : BaseEntity
{
    public const int MaxNameLength = 100;
    public const int MaxDescriptionLength = 500;
    public const int MaxDeliveryTimeLength = 100;
    private DeliveryMethod() { }
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public decimal Price { get; private set; }
    public string EstimatedDeliveryTime { get; private set; } = null!;
    public bool IsAvailable { get; private set; }
    public int DisplayOrder { get; private set; }
    public byte[] RowVersion { get; private set; } = [];

    public static Result<DeliveryMethod> Create(
        Guid id,
        string name,
        decimal price,
        string estimatedDeliveryTime,
        string? description = null,
        bool isAvailable = true,
        int displayOrder = 0)
    {
        if(id == Guid.Empty)
            return Result<DeliveryMethod>.Failure(DeliveryMethodErrors.InvalidId);
        var deliveryMethod = new DeliveryMethod() { Id = id };

        var nameResult = deliveryMethod.SetName(name);
        if(nameResult.IsFailure)
            return Result<DeliveryMethod>.Failure(nameResult.Error!);

        var priceResult = deliveryMethod.SetPrice(price);
        if (priceResult.IsFailure)
            return Result<DeliveryMethod>.Failure(priceResult.Error!);

        var estimatedDeliveryTimeResult = deliveryMethod.SetEstimatedDeliveryTime(estimatedDeliveryTime);
        if (estimatedDeliveryTimeResult.IsFailure)
            return Result<DeliveryMethod>.Failure(estimatedDeliveryTimeResult.Error!);

        var descriptionResult = deliveryMethod.SetDescription(description);
        if (descriptionResult.IsFailure)
            return Result<DeliveryMethod>.Failure(descriptionResult.Error!);

        deliveryMethod.IsAvailable = isAvailable;
        deliveryMethod.DisplayOrder = displayOrder;

        return Result<DeliveryMethod>.Success(deliveryMethod);
    }

    public Result Update(
        string name,
        decimal price,
        string estimatedDeliveryTime,
        string? description = null,
        bool isAvailable = true,
        int displayOrder = 0)
    {
        var nameResult = SetName(name);
        if (nameResult.IsFailure)
            return nameResult;

        var priceResult = SetPrice(price);
        if (priceResult.IsFailure)
            return priceResult;

        var estimatedDeliveryTimeResult = SetEstimatedDeliveryTime(estimatedDeliveryTime);
        if (estimatedDeliveryTimeResult.IsFailure)
            return estimatedDeliveryTimeResult;

        var descriptionResult = SetDescription(description);
        if (descriptionResult.IsFailure)
            return descriptionResult;

        IsAvailable = isAvailable;
        DisplayOrder = displayOrder;

        return Result.Success();
    }
    private Result SetName(string name)
    {
        var trimmed = name.Trim();
        if (string.IsNullOrWhiteSpace(trimmed))
            return Result.Failure(DeliveryMethodErrors.NameRequired);

        if (trimmed.Length > MaxNameLength)
            return Result.Failure(DeliveryMethodErrors.NameLengthExceeded);

        Name = trimmed;
        return Result.Success();
    }

    private Result SetDescription(string? description)
    {
        if (description is null)
        {
            Description = null;
            return Result.Success();
        }

        var trimmed = description.Trim();
        if (trimmed.Length > MaxDescriptionLength)
            return Result.Failure(DeliveryMethodErrors.DescriptionLengthExceeded);

        Description = trimmed;
        return Result.Success();
    }

    private Result SetPrice(decimal price)
    {
        if (price < 0)
            return Result.Failure(DeliveryMethodErrors.NegativePrice);

        Price = price;
        return Result.Success();
    }

    private Result SetEstimatedDeliveryTime(string estimatedDeliveryTime)
    {
        var trimmed = estimatedDeliveryTime.Trim();
        if (string.IsNullOrWhiteSpace(trimmed))
            return Result.Failure(DeliveryMethodErrors.EstimatedDeliveryTimeRequired);

        if (trimmed.Length > MaxDeliveryTimeLength)
            return Result.Failure(DeliveryMethodErrors.EstimatedDeliveryTimeLengthExceeded);

        EstimatedDeliveryTime = trimmed;
        return Result.Success();
    }

}