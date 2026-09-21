namespace ECommerce.API.Constants;

public static class StockMessages
{
    // Stock Retrieval Messages
    public const string StockRetrievedSuccessfully = "Stock details retrieved successfully.";
    public const string PagedStocksRetrievedSuccessfully = "Stocks retrieved successfully.";
    public const string StockNotFound = "Stock information for the specified product was not found.";

    // Stock Modification Messages
    public const string StockAdjustedSuccessfully = "Stock quantity adjusted successfully.";
    public const string InvalidStockAdjustment = "Invalid stock adjustment operation. Ensure quantity rules are met.";
    public const string InsufficientStock = "Insufficient stock available to complete this operation.";

    // Transaction Messages
    public const string TransactionsRetrievedSuccessfully = "Stock transactions retrieved successfully.";
    public const string NoTransactionsFound = "No stock transactions found for the specified product.";

    // General Validation/Business Rules Messages
    public const string LowStockWarning = "Warning: Product stock is running low.";
    public const string OutOfStock = "The requested product is currently out of stock.";
    public const string InvalidProductId = "The provided Product ID is invalid.";
}
