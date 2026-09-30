using ECommerce.Domain.Enums;
using ECommerce.UseCases.Features.Orders.Enums;

namespace ECommerce.API.Contracts.Requests.Orders;
public sealed record GetPagedOrdersByUserIdRequest(
    int PageNumber = 1,
    int PageSize = 5,
    string? Search = null,
    Guid? OrderId = null,
    Guid? ProductId = null,
    string? ProductName = null,
    string? ProductSku = null,
    Guid? DeliveryMethodId = null,
    OrderStatus? Status = null,
    OrderSortField SortBy = OrderSortField.CreatedAt,
    bool SortDescending = true);