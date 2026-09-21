using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Errors;
using ECommerce.Domain.Enums;

namespace ECommerce.Domain.Entities.StockAggregate;

public sealed class StockTransaction : BaseEntity
{
    public const int MaxNotesLength = 500;
    public int QuantityChanged { get; private set; }
    public int QuantityBefore { get; private set; }
    public int QuantityAfter { get; private set; }
    public StockTransactionType Type { get; private set; }
    public Guid? ReferenceId { get; private set; }
    public string? Notes { get; private set; }

    public Stock Stock { get; private set; } = null!;
    public Guid StockId { get; private set; }

    private StockTransaction() { }

    public static Result<StockTransaction> Create(
        Guid id,
        Guid stockId,
        int quantityChanged,
        int quantityBefore,
        int quantityAfter,
        StockTransactionType type,
        Guid? referenceId = null,
        string? notes = null)
    {
        var stockTransaction = new StockTransaction();

        var setIdsResult = stockTransaction.SetIds(id, stockId);
        if (setIdsResult.IsFailure)
            return Result<StockTransaction>.Failure(setIdsResult.Error!);

        var setQuantityResult = stockTransaction.SetQuantity(quantityChanged, quantityBefore, quantityAfter);
        if (setQuantityResult.IsFailure)
            return Result<StockTransaction>.Failure(setQuantityResult.Error!);

        var setNotesResult = stockTransaction.SetNotes(notes);
        if (setNotesResult.IsFailure)
            return Result<StockTransaction>.Failure(setNotesResult.Error!);


        if (!Enum.IsDefined(type))
            return Result<StockTransaction>.Failure(StockTransactionErrors.InvalidTransactionType);

        stockTransaction.Type = type;
        stockTransaction.ReferenceId = referenceId;

        return Result<StockTransaction>.Success(stockTransaction);
    }

    private Result SetIds(Guid id, Guid stockId)
    {
        if (id == Guid.Empty)
            return Result.Failure(StockTransactionErrors.InvalidId);

        if (stockId == Guid.Empty)
            return Result.Failure(StockTransactionErrors.InvalidStockId);

        Id = id;
        StockId = stockId;
        return Result.Success();
    }

    private Result SetQuantity(int quantityChanged, int quantityBefore, int quantityAfter)
    {
        if (quantityChanged == 0)
            return Result.Failure(StockTransactionErrors.ZeroQuantityChanged);

        if (quantityBefore < 0 || quantityAfter < 0)
            return Result.Failure(StockTransactionErrors.NegativeQuantityState);

        if (quantityBefore + quantityChanged != quantityAfter)
            return Result.Failure(StockTransactionErrors.QuantityMismatch);

        QuantityChanged = quantityChanged;
        QuantityBefore = quantityBefore;
        QuantityAfter = quantityAfter;

        return Result.Success();
    }

    private Result SetNotes(string? notes)
    {
        if (string.IsNullOrWhiteSpace(notes)) return Result.Success();


        if (notes.Length > MaxNotesLength)
            return Result.Failure(StockTransactionErrors.NotesLengthExceeded);
        Notes = notes.Trim();
        return Result.Success();

        
    }
}