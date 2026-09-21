using ECommerce.Domain.Entities.StockAggregate;

namespace ECommerce.Domain.Common.Errors;

public static class StockTransactionErrors
{
    public static readonly Error InvalidId =
        Error.Validation("StockTransaction.InvalidId", "Transaction Id is invalid.");

    public static readonly Error InvalidStockId =
        Error.Validation("StockTransaction.InvalidStockId", "Stock Id is invalid.");

    public static readonly Error ZeroQuantityChanged =
        Error.Validation("StockTransaction.ZeroQuantityChanged", "Quantity changed cannot be zero.");

    public static readonly Error NegativeQuantityState =
        Error.Validation("StockTransaction.NegativeQuantityState", "Stock quantities before or after cannot be negative.");

    public static readonly Error InvalidTransactionType =
        Error.Validation("StockTransaction.InvalidType", "Invalid transaction type specified.");

    public static readonly Error NotFound =
        Error.NotFound("StockTransaction.NotFound", "Stock transaction history not found.");

    public static readonly Error QuantityMismatch =
        Error.Validation(
            "StockTransaction.QuantityMismatch",
            "Quantity after change must equal quantity before plus quantity changed.");

    public static readonly Error NotesLengthExceeded =
        Error.Validation("Product.NameLengthExceeded", $"Transaction notes cannot exceed {StockTransaction.MaxNotesLength} characters.");
}