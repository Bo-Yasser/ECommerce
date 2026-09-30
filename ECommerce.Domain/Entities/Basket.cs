using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Errors;
using System.Text.Json.Serialization;

namespace ECommerce.Domain.Entities;

public class Basket
{
    public Guid BuyerId { get; private set; }

    private readonly List<BasketItem> _items = [];
    public IReadOnlyList<BasketItem> Items => _items.AsReadOnly();

    public int TotalItems => _items.Sum(item => item.Quantity);
    public decimal SubTotal => _items.Sum(item => item.LineTotal);

    [JsonConstructor]
    private Basket(Guid buyerId, IReadOnlyList<BasketItem> items)
    {
        BuyerId = buyerId;
        _items = items.ToList();
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

        _items.Add(createResult.Value);
        return Result.Success();
    }
    public Result RemoveItem(Guid productId)
    {
        var existingItemResult = GetItem(productId);

        if (existingItemResult.IsFailure)
            return Result.Failure(existingItemResult.Error!);

        _items.Remove(existingItemResult.Value);
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
            var existingItemResult = GetItem(item.ProductId);
            if (existingItemResult.IsFailure)
            {
                var createItemResult = BasketItem.Create(
                    item.ProductId,
                    item.ProductName,
                    item.PictureUrl,
                    item.UnitPrice,
                    item.Quantity);
                if (createItemResult.IsFailure)
                    return Result.Failure(createItemResult.Error!);

                _items.Add(createItemResult.Value);
                continue;
            }

            var mergedQuantity = Math.Min(
                existingItemResult.Value.Quantity + item.Quantity,
                BasketItem.MaxQuantity);

            var result = existingItemResult.Value.SetQuantity(mergedQuantity);
            if (result.IsFailure)
                return result;
        }
        return Result.Success();
    }
    public void Clear() => _items.Clear();

    public int GetItemQuantity(Guid productId)
        => _items.FirstOrDefault(item => item.ProductId == productId)?.Quantity ?? 0;
    private Result<BasketItem> GetItem(Guid productId)
    {
        var item = _items.FirstOrDefault(item => item.ProductId == productId);
        if (item is null)
            return Result<BasketItem>.Failure(BasketErrors.ItemNotFound);

        return Result<BasketItem>.Success(item);
    }
}
