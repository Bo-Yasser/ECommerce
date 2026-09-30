using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Errors;
using ECommerce.Domain.Entities.OrderAggregate;
using ECommerce.Domain.Repositories;
using ECommerce.UseCases.Common.Interfaces;
using ECommerce.UseCases.Common.Pagination;
using ECommerce.UseCases.Features.Orders.Models;
using ECommerce.UseCases.Features.Orders.Responses;
using ECommerce.UseCases.Features.Orders.Specifications;
using MediatR;

namespace ECommerce.UseCases.Features.Orders.Queries.GetPagedOrdersByUserId;

public sealed class GetPagedOrdersByUserIdQueryHandler(
    IReadRepository<Order> repository,
    ICurrentUserService currentUserService) : IRequestHandler<GetPagedOrdersByUserIdQuery, Result<PagedResult<OrderResponse>>>
{
    public async Task<Result<PagedResult<OrderResponse>>> Handle(GetPagedOrdersByUserIdQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;
        if (userId is null)
            return Result<PagedResult<OrderResponse>>.Failure(OrderErrors.TokenMissing);

        var filters = request.Filters is not null 
            ? request.Filters with { UserId = userId } 
            : new OrderFilters(UserId: userId);

        var countSpec = new OrdersFilterSpecification(
            filters: filters);

        var listSpec = new PagedOrdersSpecification(
            filters: filters,
            sortBy: request.SortBy,
            sortDescending: request.SortDescending,
            pageNumber: request.PageNumber,
            pageSize: request.PageSize);

        var ordersCount = await repository.CountAsync(countSpec, cancellationToken);
        var orders = await repository.ListAsync(listSpec, cancellationToken);

        return Result<PagedResult<OrderResponse>>.Success(new PagedResult<OrderResponse>(orders, ordersCount));
    }
}
