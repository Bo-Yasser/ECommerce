using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Errors;
using ECommerce.Domain.Entities.OrderAggregate;
using ECommerce.Domain.Repositories;
using ECommerce.UseCases.Common.Interfaces;
using ECommerce.UseCases.Features.Orders.Responses;
using ECommerce.UseCases.Features.Orders.Specifications;
using MediatR;

namespace ECommerce.UseCases.Features.Orders.Queries.GetOrderByIdForUser;

public sealed class GetOrderByIdForUserQueryHandler(
    ICurrentUserService currentUserService,
    IReadRepository<Order> repository) 
    : IRequestHandler<GetOrderByIdForUserQuery, Result<OrderResponse>>
{
    public async Task<Result<OrderResponse>> Handle(GetOrderByIdForUserQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;
        if (userId is null)
            return Result<OrderResponse>.Failure(OrderErrors.TokenMissing);

        var order = await repository.FirstOrDefaultAsync(
            new OrderWithItemsByIdSpecification(request.Id, userId.Value, asTracking: false),
            cancellationToken);

        if(order is null)
            return Result<OrderResponse>.Failure(OrderErrors.NotFound);

        return Result<OrderResponse>.Success(OrderResponse.From(order));

    }
}
