namespace ECommerce.Domain.Enums;

public enum StockTransactionType
{
    Initial = 0,
    Addition = 1,
    OrderDeduction = 2,
    OrderCancellation = 3,
    OrderReturn = 4,
    DamageLoss = 5,
    ManualAdjustment = 6
}

