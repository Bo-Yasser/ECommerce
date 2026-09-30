using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Errors;
using ECommerce.Domain.Entities.OrderAggregate;
using ECommerce.Domain.Repositories;
using ECommerce.UseCases.Features.Orders.Responses;
using ECommerce.UseCases.Features.Orders.Specifications;
using MediatR;

namespace ECommerce.UseCases.Features.Orders.Queries.GetOrderById;

public sealed class GetOrderByIdQueryHandler(IReadRepository<Order> repository)
    : IRequestHandler<GetOrderByIdQuery, Result<OrderResponse>>
{
    public async Task<Result<OrderResponse>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await repository.FirstOrDefaultAsync(
            new OrderWithItemsByIdSpecification(request.Id, asTracking: false),
            cancellationToken);

        if (order is null)
            return Result<OrderResponse>.Failure(OrderErrors.NotFound);

        return Result<OrderResponse>.Success(OrderResponse.From(order));
    }
}
