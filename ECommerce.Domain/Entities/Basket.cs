using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Errors;
using System.Text.Json.Serialization;

namespace ECommerce.Domain.Entities;

public class Basket
{
    public Guid BuyerId { get; private set; }
    public List<BasketItem> Items { get; private set; } = [];

    public int TotalItems => Items.Sum(item => item.Quantity);
    public decimal SubTotal => Items.Sum(item => item.LineTotal);

    [JsonConstructor]
    private Basket(Guid buyerId, List<BasketItem> items)
    {
        BuyerId = buyerId;
        Items = items;
    }

    public static Result<Basket> Create(
        Guid buyerId,
        List<BasketItem> items)
    {
        if (buyerId == Guid.Empty)
            return Result<Basket>.Failure(BasketErrors.InvalidBuyerId);

        var basket = new Basket(buyerId, items);

        return Result<Basket>.Success(basket);
    }

    public static Result<Basket> CreateEmpty(Guid buyerId)
    {
        if (buyerId == Guid.Empty)
            return Result<Basket>.Failure(BasketErrors.InvalidBuyerId);

        var basket = new Basket(buyerId, []);

        return Result<Basket>.Success(basket);
    }

    public Result AddItem(
        Guid productId,
        string productName,
        string pictureUrl,
        decimal unitPrice,
        int quantity)
    {
        var existingItemResult = GetItem(productId);

        if (existingItemResult.IsSuccess)
        {
            var increaseQuantityResult = existingItemResult.Value.IncreaseQuantity(quantity);
            if (increaseQuantityResult.IsFailure)
            {
                return Result.Failure(increaseQuantityResult.Error!);
            }

            return Result.Success();
        }

        var createResult = BasketItem.Create(
            productId: productId,
            productName: productName,
            pictureUrl: pictureUrl,
            unitPrice: unitPrice,
            quantity: quantity);

        if (createResult.IsFailure)
        {
            return Result.Failure(createResult.Error!);
        }

        Items.Add(createResult.Value);
        return Result.Success();
    }
    public Result RemoveItem(Guid productId)
    {
        var existingItemResult = GetItem(productId);

        if (existingItemResult.IsFailure)
            return Result.Failure(existingItemResult.Error!);

        Items.Remove(existingItemResult.Value);
        return Result.Success();

    }
    public Result UpdateItemQuantity(Guid productId, int quantity)
    {
        var itemResult = GetItem(productId);
        if (itemResult.IsFailure)
            return Result.Failure(itemResult.Error!);

        return itemResult.Value.SetQuantity(quantity);
    }
    
    public Result MergeFrom(Basket other)
    {
        if (other.BuyerId == BuyerId)
            return Result.Failure(BasketErrors.CannotMergeSameBuyer);

        foreach(var item in other.Items)
        {
            var mergeResult = AddItem(
                productId: item.ProductId,
                productName: item.ProductName,
                pictureUrl: item.PictureUrl,
                unitPrice: item.UnitPrice,
                quantity: item.Quantity);

            if (mergeResult.IsFailure) return mergeResult;
        }
        return Result.Success();
    }
    public void Clear() => Items.Clear(); 

    private Result<BasketItem> GetItem(Guid productId)
    {
        var item = Items.FirstOrDefault(item => item.ProductId == productId);
        if (item is null)
            return Result<BasketItem>.Failure(BasketErrors.ItemNotFound);

        return Result<BasketItem>.Success(item);
    }
}
