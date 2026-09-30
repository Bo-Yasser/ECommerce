using ECommerce.Domain.Entities.OrderAggregate;
using ECommerce.UseCases.Specifications;

namespace ECommerce.UseCases.Features.Orders.Specifications;

public sealed class OrderWithItemsByIdSpecification : Specification<Order>
{
    public OrderWithItemsByIdSpecification(Guid orderId, Guid? userId = null, bool asTracking = true)
    {
        var query = Query;

        query
            .Where(o => o.Id == orderId)
            .Include(o => o.Items);

        if (userId is not null)
            query.Where(o => o.UserId == userId);

        if (asTracking)
            query.AsTracking();
    }
}