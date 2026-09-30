using ECommerce.Domain.Common;
using ECommerce.UseCases.Common.Pagination;
using ECommerce.UseCases.Features.Orders.Enums;
using ECommerce.UseCases.Features.Orders.Models;
using ECommerce.UseCases.Features.Orders.Responses;
using MediatR;

namespace ECommerce.UseCases.Features.Orders.Queries.GetPagedOrdersByUserId;

public sealed record GetPagedOrdersByUserIdQuery(
    int PageNumber = 1,
    int PageSize = 5,
    OrderFilters? Filters = null,
    OrderSortField SortBy = OrderSortField.CreatedAt,
    bool SortDescending = true) : IRequest<Result<PagedResult<OrderResponse>>>;
