using ECommerce.Domain.Entities.OrderAggregate;
using ECommerce.UseCases.Features.Orders.Enums;
using ECommerce.UseCases.Features.Orders.Models;
using ECommerce.UseCases.Features.Orders.Responses;
using ECommerce.UseCases.Specifications;

namespace ECommerce.UseCases.Features.Orders.Specifications;

public sealed class PagedOrdersSpecification : Specification<Order, OrderResponse>
{
    public PagedOrdersSpecification(
        OrderFilters? filters = null,
        OrderSortField sortBy = OrderSortField.CreatedAt,
        bool sortDescending = true,
        int? pageNumber = null,
        int? pageSize = null)
    {
        var query = Query;

        query = query.ApplyFilters(filters);

        ApplySort(query, sortBy, sortDescending);

        if(pageNumber.HasValue && pageSize.HasValue)
        {
            var skip = (pageNumber.Value - 1) * pageSize.Value;
            query
                .Skip(skip)
                .Take(pageSize.Value);
        }


        query.Select(o => new OrderResponse(
            o.Id,
            o.UserId,
            o.Status,
            o.DeliveryMethodId,
            new OrderDeliveryMethodResponse(
                o.DeliveryMethod.DeliveryMethodName,
                o.DeliveryMethod.DeliveryMethodPrice,
                o.DeliveryMethod.DeliveryMethodEstimatedTime),
            new ShippingAddressResponse(
                o.ShippingAddress.RecipientFirstName,
                o.ShippingAddress.RecipientLastName,
                o.ShippingAddress.PhoneNumber,
                o.ShippingAddress.Country,
                o.ShippingAddress.City,
                o.ShippingAddress.Street,
                o.ShippingAddress.PostalCode),
            o.SubTotal,
            o.ShippingCost,
            o.Total,
            o.CreatedAt,
            o.Items.Select(i => new OrderItemResponse(
                i.Id,
                i.ProductId,
                new ProductItemOrderedResponse(
                    i.ItemOrdered.Sku,
                    i.ItemOrdered.ProductName,
                    i.ItemOrdered.PictureUrl,
                    i.ItemOrdered.UnitPrice),
                i.Quantity,
                i.ItemOrdered.UnitPrice * i.Quantity)
            ).ToList(),
            o.RowVersion));

    }
    private void ApplySort(
        ISpecificationBuilder<Order, OrderResponse> query,
        OrderSortField sortBy,
        bool sortDescending)
    {
        switch (sortBy)
        {
            case OrderSortField.Total:
                if (sortDescending)
                    query.OrderByDescending(o => o.Total)
                        .ThenByDescending(o => o.CreatedAt)
                        .ThenByDescending(o => o.Id);

                else
                    query.OrderBy(o => o.Total)
                        .ThenBy(o => o.CreatedAt)
                        .ThenBy(o => o.Id);
                break;

            case OrderSortField.SubTotal:
                if (sortDescending)
                    query.OrderByDescending(o => o.SubTotal)
                        .ThenByDescending(o => o.CreatedAt)
                        .ThenByDescending(o => o.Id);

                else
                    query.OrderBy(o => o.SubTotal)
                        .ThenBy(o => o.CreatedAt)
                        .ThenBy(o => o.Id);

                break;

            case OrderSortField.Status:
                if (sortDescending)
                    query.OrderByDescending(o => o.Status)
                        .ThenByDescending(o => o.CreatedAt)
                        .ThenByDescending(o => o.Id);
                else
                    query.OrderBy(o => o.Status)
                        .ThenBy(o => o.CreatedAt)
                        .ThenBy(o => o.Id);
                break;

            case OrderSortField.TotalQuantity:
                if (sortDescending)
                    query.OrderByDescending(o => o.Items.Sum(i => i.Quantity))
                        .ThenByDescending(o => o.CreatedAt)
                        .ThenByDescending(o => o.Id);
                else
                    query.OrderBy(o => o.Items.Sum(i => i.Quantity))
                        .ThenBy(o => o.CreatedAt)
                        .ThenBy(o => o.Id);
                break;

            case OrderSortField.Address:
                if (sortDescending)
                    query.OrderByDescending(o => o.ShippingAddress.Country)
                        .ThenByDescending(o => o.ShippingAddress.City)
                        .ThenByDescending(o => o.ShippingAddress.PostalCode)
                        .ThenByDescending(o => o.ShippingAddress.RecipientFirstName)
                        .ThenByDescending(o => o.ShippingAddress.RecipientLastName)
                        .ThenByDescending(o => o.Id);
                else
                    query.OrderBy(p => p.ShippingAddress.Country)
                        .ThenBy(o => o.ShippingAddress.City)
                        .ThenBy(o => o.ShippingAddress.PostalCode)
                        .ThenBy(o => o.ShippingAddress.RecipientFirstName)
                        .ThenBy(o => o.ShippingAddress.RecipientLastName)
                        .ThenBy(o => o.Id);
                break;

            case OrderSortField.DeliveryMethodName:
                if (sortDescending)
                    query.OrderByDescending(o => o.DeliveryMethod.DeliveryMethodName)
                        .ThenByDescending(o => o.DeliveryMethod.DeliveryMethodEstimatedTime)
                        .ThenByDescending(o => o.CreatedAt)
                        .ThenByDescending(o => o.Id);
                else
                    query.OrderBy(p => p.DeliveryMethod.DeliveryMethodName)
                        .ThenBy(o => o.DeliveryMethod.DeliveryMethodEstimatedTime)
                        .ThenBy(o => o.CreatedAt)
                        .ThenBy(o => o.Id);
                break;

            case OrderSortField.CreatedAt:
                if (sortDescending)
                    query.OrderByDescending(o => o.CreatedAt)
                        .ThenByDescending(o => o.Status)
                        .ThenByDescending(o => o.Id);
                else
                    query.OrderBy(p => p.CreatedAt)
                        .ThenBy(o => o.Status)
                        .ThenBy(o => o.Id);
                break;
        }
    }
}
