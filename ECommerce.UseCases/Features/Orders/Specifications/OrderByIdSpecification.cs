using ECommerce.Domain.Entities.OrderAggregate;
using ECommerce.UseCases.Specifications;

namespace ECommerce.UseCases.Features.Orders.Specifications;

public sealed class OrderByIdSpecification : Specification<Order>
{
    public OrderByIdSpecification(Guid id)
        => Query.Where(o => o.Id == id).AsTracking();
}