using ECommerce.Domain.Entities.OrderAggregate;
using ECommerce.UseCases.Features.Orders.Models;
using ECommerce.UseCases.Specifications;

namespace ECommerce.UseCases.Features.Orders.Specifications;
public sealed class OrdersFilterSpecification : Specification<Order>
{
    public OrdersFilterSpecification(OrderFilters? filters)
        => Query.ApplyFilters(filters);

}