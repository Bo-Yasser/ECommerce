namespace ECommerce.Domain.Enums;

public enum StockTransactionType
{
    Initial = 0,
    Addition = 1,
    OrderDeduction = 2,
    OrderReturn = 3,
    DamageLoss = 4,
    ManualAdjustment = 5
}
