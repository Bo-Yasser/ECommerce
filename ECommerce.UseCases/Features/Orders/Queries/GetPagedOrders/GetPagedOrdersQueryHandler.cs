using ECommerce.Domain.Common;
using ECommerce.Domain.Entities.OrderAggregate;
using ECommerce.Domain.Repositories;
using ECommerce.UseCases.Common.Pagination;
using ECommerce.UseCases.Features.Orders.Responses;
using ECommerce.UseCases.Features.Orders.Specifications;
using MediatR;

namespace ECommerce.UseCases.Features.Orders.Queries.GetPagedOrders;

public sealed class GetPagedOrdersQueryHandler(IReadRepository<Order> repository)
    : IRequestHandler<GetPagedOrdersQuery, Result<PagedResult<OrderResponse>>>
{
    public async Task<Result<PagedResult<OrderResponse>>> Handle(GetPagedOrdersQuery request, CancellationToken cancellationToken)
    {
        var countSpec = new OrdersFilterSpecification(
            filters: request.Filters);

        var listSpec = new PagedOrdersSpecification(
            filters: request.Filters,
            sortBy: request.SortBy,
            sortDescending: request.SortDescending,
            pageNumber: request.PageNumber,
            pageSize: request.PageSize);

        var ordersCount = await repository.CountAsync(countSpec, cancellationToken);
        var orders = await repository.ListAsync(listSpec, cancellationToken);

        return Result<PagedResult<OrderResponse>>.Success(new PagedResult<OrderResponse>(orders, ordersCount));
    }
}
