using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Errors;
using ECommerce.Domain.Enums;

namespace ECommerce.Domain.Entities.StockAggregate;

public sealed class Stock : BaseEntity
{
    public const int LowStockThreshold = 10;
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }
    public byte[] RowVersion { get; private set; } = [];

    public Product Product { get; private set; } = null!;

    private readonly List<StockTransaction> _transactions = [];
    public IReadOnlyList<StockTransaction> Transactions => _transactions.AsReadOnly();
    private Stock() { }

    public static Result<Stock> Create(
        Guid id,
        Guid productId,
        int initialQuantity = 0,
        Guid? referenceId = null)
    {
        var stock = new Stock();

        var setIdResult = stock.SetId(id);
        if (setIdResult.IsFailure)
            return Result<Stock>.Failure(setIdResult.Error!);

        var setProductResult = stock.SetProduct(productId);
        if (setProductResult.IsFailure)
            return Result<Stock>.Failure(setProductResult.Error!);

        if (initialQuantity < 0)
            return Result<Stock>.Failure(StockErrors.NegativeQuantity);

        stock.Quantity = initialQuantity;

        if (initialQuantity > 0)
        {
            var initialTransaction = StockTransaction.Create(
                id: Guid.NewGuid(),
                stockId: stock.Id,
                quantityBefore: 0,
                quantityChanged: initialQuantity,
                quantityAfter: initialQuantity,
                type: StockTransactionType.Initial,
                notes: "Initial Stock Creation",
                referenceId: referenceId);

            if (initialTransaction.IsSuccess)
                stock._transactions.Add(initialTransaction.Value!);
        }

        return Result<Stock>.Success(stock);
    }

    public Result Deduct(
        int amount,
        StockTransactionType type,
        string? notes = null,
        Guid? referenceId = null)
    {
        if (amount <= 0)
            return Result.Failure(StockErrors.InvalidAmount);

        if (Quantity < amount)
            return Result.Failure(StockErrors.InsufficientStock);

        int quantityBefore = Quantity;
        Quantity -= amount;

        AddTransaction(quantityBefore, -amount, Quantity, type, notes, referenceId);

        return Result.Success();
    }

    public Result Restore(
        int amount,
        StockTransactionType type,
        string? notes = null,
        Guid? referenceId = null)
    {
        if (amount <= 0)
            return Result.Failure(StockErrors.InvalidAmount);

        int quantityBefore = Quantity;
        Quantity += amount;

        AddTransaction(quantityBefore, amount, Quantity, type, notes, referenceId);

        return Result.Success();
    }

    public Result AdjustQuantity(
        int newQuantity,
        string? notes = null,
        Guid? referenceId = null)
    {
        if (newQuantity < 0)
            return Result.Failure(StockErrors.NegativeQuantity);

        if (newQuantity == Quantity)
            return Result.Success();

        int quantityBefore = Quantity;
        int quantityChanged = newQuantity - quantityBefore;
        Quantity = newQuantity;

        AddTransaction(
            quantityBefore,
            quantityChanged,
            Quantity,
            StockTransactionType.ManualAdjustment,
            notes,
            referenceId);

        return Result.Success();
    }

    private void AddTransaction(
        int quantityBefore,
        int quantityChanged,
        int quantityAfter,
        StockTransactionType type,
        string? notes = null,
        Guid? referenceId = null)
    {
        var transactionResult = StockTransaction.Create(
            id: Guid.NewGuid(),
            stockId: Id,
            quantityBefore: quantityBefore,
            quantityChanged: quantityChanged,
            quantityAfter: quantityAfter,
            type: type,
            notes: notes,
            referenceId: referenceId
        );

        if (transactionResult.IsSuccess)
        {
            _transactions.Add(transactionResult.Value!);
        }
    }

    private Result SetProduct(Guid productId)
    {
        if (productId == Guid.Empty)
            return Result.Failure(StockErrors.InvalidProductId);

        ProductId = productId;
        return Result.Success();
    }

    private Result SetId(Guid id)
    {
        if (id == Guid.Empty)
            return Result.Failure(StockErrors.InvalidId);

        Id = id;
        return Result.Success();
    }
}