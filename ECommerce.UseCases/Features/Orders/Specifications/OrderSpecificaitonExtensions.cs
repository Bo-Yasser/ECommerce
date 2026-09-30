using ECommerce.Domain.Common;
using ECommerce.Domain.Entities.OrderAggregate;
using ECommerce.Domain.Enums;
using ECommerce.UseCases.Features.Orders.Models;
using ECommerce.UseCases.Specifications;
using System.Linq.Expressions;

namespace ECommerce.UseCases.Features.Orders.Specifications;
public static class OrderSpecificationExtensions
{
    public static ISpecificationBuilder<Order> ApplyFilters(
        this ISpecificationBuilder<Order> query,
        OrderFilters? filters)
    {
        return ApplyOrderFilters(
            query,
            filters,
            static (q, predicate) => q.Where(predicate));
    }

    public static ISpecificationBuilder<Order, TResult> ApplyFilters<TResult>(
        this ISpecificationBuilder<Order, TResult> query,
        OrderFilters? filters)
    {
        return ApplyOrderFilters(
            query,
            filters,
            static (q, predicate) => q.Where(predicate));
    }

    private static TBuilder ApplyOrderFilters<TBuilder>(
        TBuilder query,
        OrderFilters? filters,
        Func<TBuilder, Expression<Func<Order, bool>>, TBuilder> where)
    {
        if (filters is null)
            return query;

        if (!string.IsNullOrWhiteSpace(filters.Search))
        {
            var term = filters.Search.Trim().ToLower();

            query = where(query, o =>
                o.Items.Any(i =>
                    i.ItemOrdered.ProductName.Contains(term) ||
                    i.ItemOrdered.Sku.Contains(term)) ||

                o.DeliveryMethod.DeliveryMethodName.Contains(term) ||

                o.ShippingAddress.RecipientFirstName.Contains(term) ||
                o.ShippingAddress.RecipientLastName.Contains(term) ||
                o.ShippingAddress.Country.Contains(term) ||
                o.ShippingAddress.City.Contains(term));
        }

        if (filters.OrderId.HasValue)
            query = where(query, o =>
                o.Id == filters.OrderId.Value);

        if (filters.UserId.HasValue)
            query = where(query, o =>
                o.UserId == filters.UserId.Value);

        if (filters.ProductId.HasValue)
            query = where(query, o =>
                o.Items.Any(i =>
                    i.ProductId == filters.ProductId.Value));

        if (!string.IsNullOrWhiteSpace(filters.ProductName))
        {
            var productName = filters.ProductName.Trim();

            query = where(query, o =>
                o.Items.Any(i =>
                    i.ItemOrdered.ProductName == productName));
        }

        if (!string.IsNullOrWhiteSpace(filters.ProductSku))
        {
            var productSku = filters.ProductSku.Trim();

            query = where(query, o =>
                o.Items.Any(i => i.ItemOrdered.Sku == productSku));
        }

        if (filters.DeliveryMethodId.HasValue)
            query = where(query, o =>
                o.DeliveryMethodId == filters.DeliveryMethodId.Value);

        if (filters.Status is OrderStatus status)
            query = where(query, o =>
                o.Status == status);
        return query;
    }
}
