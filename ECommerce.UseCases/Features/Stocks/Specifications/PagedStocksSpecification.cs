using ECommerce.Domain.Entities.StockAggregate;
using ECommerce.UseCases.Features.Stocks.Enums;
using ECommerce.UseCases.Features.Stocks.Models;
using ECommerce.UseCases.Features.Stocks.Responses;
using ECommerce.UseCases.Specifications;

namespace ECommerce.UseCases.Features.Stocks.Specifications;

public sealed class PagedStocksSpecification : Specification<Stock, StockResponse>
{
    public PagedStocksSpecification(
        StockFilters? filters = null,
        StockSortField sortBy = StockSortField.ProductName,
        bool sortDescending = false,
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

        query.Select(s => new StockResponse(
            s.Id,
            s.ProductId,
            s.Product.Sku,
            s.Product.Name,
            s.Quantity,
            s.RowVersion));
    }

    private void ApplySort(
        ISpecificationBuilder<Stock, StockResponse> query,
        StockSortField sortBy,
        bool sortDescending)
    {
        switch (sortBy)
        {
            case StockSortField.ProductName:
                if (sortDescending)
                {
                    query.OrderByDescending(s => s.Product.Name)
                        .ThenByDescending(s => s.CreatedAt)
                        .ThenByDescending(s => s.Id);

                }
                else
                {
                    query.OrderBy(s => s.Product.Name)
                        .ThenBy(s => s.CreatedAt)
                        .ThenBy(s => s.Id);
                }
                break;

            case StockSortField.Quantity:
                if (sortDescending)
                {
                    query.OrderByDescending(s => s.Quantity)
                        .ThenByDescending(s => s.Product.Name)
                        .ThenByDescending(s => s.Id);
                }
                else
                {
                    query.OrderBy(s => s.Quantity)
                        .ThenBy(s => s.Product.Name)
                        .ThenBy(s => s.Id);
                }
                break;
        }
    }
}
