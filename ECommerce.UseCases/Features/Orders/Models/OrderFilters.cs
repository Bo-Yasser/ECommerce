using ECommerce.Domain.Enums;

namespace ECommerce.UseCases.Features.Orders.Models;

public sealed record OrderFilters(
    string? Search = null,
    Guid? OrderId = null,
    Guid? UserId = null,
    Guid? ProductId = null,
    string? ProductName = null,
    string? ProductSku = null,
    Guid? DeliveryMethodId = null,
    OrderStatus? Status = null);