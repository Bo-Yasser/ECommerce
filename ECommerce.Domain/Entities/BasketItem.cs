using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Errors;
using System.Text.Json.Serialization;

namespace ECommerce.Domain.Entities;

public class BasketItem
{
    public const int MinQuantity = 1;
    public const int MaxQuantity = 99;
    public decimal LineTotal => UnitPrice * Quantity;


    public Guid ProductId { get; private set; }
    public string ProductName { get; private set; } = null!;
    public string PictureUrl { get; private set; } = null!;
    public decimal UnitPrice { get; private set; }
    public int Quantity { get; private set; }

    private BasketItem()
    {

    }

    [JsonConstructor]
    private BasketItem(Guid productId, string productName, string pictureUrl, decimal unitPrice, int quantity)
    {
        ProductId = productId;
        ProductName = productName;
        PictureUrl = pictureUrl;
        UnitPrice = unitPrice;
        Quantity = quantity;
    }

    public static Result<BasketItem> Create(
        Guid productId,
        string productName,
        string pictureUrl,
        decimal unitPrice,
        int quantity)
    {
        if (productId == Guid.Empty) 
            return Result<BasketItem>.Failure(BasketErrors.InvalidProductId);

        var basketItem = new BasketItem() { ProductId = productId };

        var productNameResult = basketItem.SetProductName(productName);
        if(productNameResult.IsFailure) 
            return Result<BasketItem>.Failure(productNameResult.Error!);

        var pictureUrlResult = basketItem.SetPictureUrl(pictureUrl);
        if(pictureUrlResult.IsFailure)
            return Result<BasketItem>.Failure(pictureUrlResult.Error!);

        var unitPriceResult = basketItem.SetUnitPrice(unitPrice);
        if(unitPriceResult.IsFailure)
            return Result<BasketItem>.Failure(unitPriceResult.Error!);

        var quantityResult = basketItem.SetQuantity(quantity);
        if(quantityResult.IsFailure)
            return Result<BasketItem>.Failure(quantityResult.Error!);

        return Result<BasketItem>.Success(basketItem);
    }

    public Result Update(
        string productName,
        string pictureUrl,
        decimal unitPrice,
        int quantity)
    {
        var productNameResult = SetProductName(productName);
        if (productNameResult.IsFailure)
            return productNameResult;

        var pictureUrlResult = SetPictureUrl(pictureUrl);
        if (pictureUrlResult.IsFailure)
            return pictureUrlResult;

        var unitPriceResult = SetUnitPrice(unitPrice);
        if (unitPriceResult.IsFailure)
            return unitPriceResult;

        var quantityResult = SetQuantity(quantity);
        if (quantityResult.IsFailure)
            return quantityResult;

        return Result.Success();
    }

    private Result SetProductName(string productName)
    {
        if (string.IsNullOrWhiteSpace(productName)) 
            return Result.Failure(BasketErrors.InvalidProductName);
        if (productName.Length > Entities.Product.MaxNameLength) 
            return Result.Failure(BasketErrors.InvalidProductName);

        ProductName = productName.Trim();
        return Result.Success();
    }

    private Result SetPictureUrl(string pictureUrl)
    {
        if (string.IsNullOrWhiteSpace(pictureUrl)) 
            return Result.Failure(BasketErrors.InvalidPictureUrl);
        if (pictureUrl.Length > Entities.Product.MaxPictureUrlLength) 
            return Result.Failure(BasketErrors.InvalidPictureUrl);
        PictureUrl = pictureUrl.Trim();
        return Result.Success();
    }

    public Result SetUnitPrice(decimal unitPrice)
    {
        if (unitPrice <= 0) 
            return Result.Failure(BasketErrors.InvalidUnitPrice);
        UnitPrice = unitPrice;
        return Result.Success();
    }

    public Result SetQuantity(int quantity)
    {
        if (quantity is < MinQuantity or > MaxQuantity) 
            return Result.Failure(BasketErrors.InvalidQuantity);
        Quantity = quantity;
        return Result.Success();
    }

    public Result IncreaseQuantity(int amount)
    {
        if (amount <= 0)
            return Result.Failure(BasketErrors.InvalidQuantityIncrement);

        var newQuantity = Quantity + amount;

        if (newQuantity > MaxQuantity)
            return Result.Failure(BasketErrors.QuantityTooHigh);

        Quantity = newQuantity;
        return Result.Success();
    }

}
