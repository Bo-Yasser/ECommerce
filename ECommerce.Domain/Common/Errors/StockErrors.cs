namespace ECommerce.Domain.Common.Errors;
public static class StockErrors
{
    public static readonly Error InvalidId =
        Error.Validation("Stock.InvalidId", "Stock Id is invalid.");

    public static readonly Error InvalidProductId =
        Error.Validation("Stock.InvalidProductId", "Product Id is invalid.");

    public static readonly Error NegativeQuantity =
        Error.Validation("Stock.NegativeQuantity", "Stock quantity cannot be negative.");

    public static readonly Error InvalidAmount =
        Error.Validation("Stock.InvalidAmount", "Amount must be greater than zero.");

    public static readonly Error InsufficientStock =
        Error.Validation("Stock.InsufficientStock", "Insufficient stock quantity available.");

    public static readonly Error NotFound =
        Error.NotFound("Stock.NotFound", "Stock not found.");

    public static readonly Error OutOfStock =
        Error.Validation("Stock.OutOfStock", "Product is out of stock.");

    public static readonly Error AlreadyExists =
        Error.Conflict("Stock.AlreadyExists", "Stock record already exists for this product.");

    public static readonly Error ConcurrencyConflict =
        Error.Conflict("Stock.ConcurrencyConflict", "Stock was updated by another process. Please refresh and try again.");

    public static readonly Error CreateFailed =
        Error.Failure("Stock.CreateFailed", "Stock could not be created.");

    public static readonly Error UpdateFailed =
        Error.Failure("Stock.UpdateFailed", "Stock could not be updated.");

    public static readonly Error DeleteFailed =
        Error.Failure("Stock.DeleteFailed", "Stock could not be deleted.");
}