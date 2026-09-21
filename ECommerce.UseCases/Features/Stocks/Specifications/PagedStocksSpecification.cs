using ECommerce.Domain.Entities.StockAggregate;
using ECommerce.UseCases.Features.Stocks.Enums;
using ECommerce.UseCases.Features.Stocks.Responses;
using ECommerce.UseCases.Specifications;

namespace ECommerce.UseCases.Features.Stocks.Specifications;

public sealed class PagedStocksSpecification : Specification<Stock, StockResponse>
{
    public PagedStocksSpecification(
        string? search = null,
        Guid? productId = null,
        StockFilter? filter = StockFilter.All,
        StockSortField? sortBy = StockSortField.ProductName,
        bool sortDescending = false,
        int? pageNumber = null,
        int? pageSize = null)
    {
        var query = Query;

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLower();
            query.Where(s => s.Product.Sku.Contains(term)
                        || s.Product.Name.Contains(term)
                        || s.Product.Description.Contains(term));
        }

        if (productId.HasValue)
            query.Where(s => s.ProductId == productId.Value);

        if(filter is StockFilter statusField)
        {
            ApplyStatusFilter(query, statusField);
        }

        if (sortBy is StockSortField sortField)
        {
            ApplySort(query, sortField, sortDescending);
        }

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
        StockSortField? sortBy,
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
                        .ThenByDescending(s => s.Id);
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

    private void ApplyStatusFilter(
        ISpecificationBuilder<Stock, StockResponse> query,
        StockFilter filter)
    {
        switch (filter)
        {
            case StockFilter.InStock:
                query.Where(s => s.Quantity > 0);
                break;

            case StockFilter.LowStock:
                query.Where(s => s.Quantity > 0 && s.Quantity <= Stock.LowStockThreshold);
                break;

            case StockFilter.SufficientStock:
                query.Where(s => s.Quantity > Stock.LowStockThreshold);
                break;

            case StockFilter.OutOfStock:
                query.Where(s => s.Quantity == 0);
                break;
        }
    }
}
