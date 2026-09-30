using ECommerce.Domain.Entities.StockAggregate;
using ECommerce.UseCases.Features.Stocks.Enums;
using ECommerce.UseCases.Features.Stocks.Models;
using ECommerce.UseCases.Specifications;
using System.Linq.Expressions;

namespace ECommerce.UseCases.Features.Stocks.Specifications;
public static class StockSpecificationExtensions
{
    public static ISpecificationBuilder<Stock> ApplyFilters(
        this ISpecificationBuilder<Stock> query,
        StockFilters? filters)
    {
        return ApplyStockFilters(
            query,
            filters,
            static (q, predicate) => q.Where(predicate));
    }

    public static ISpecificationBuilder<Stock, TResult> ApplyFilters<TResult>(
        this ISpecificationBuilder<Stock, TResult> query,
        StockFilters? filters)
    {
        return ApplyStockFilters(
            query,
            filters,
            static (q, predicate) => q.Where(predicate));
    }

    private static TBuilder ApplyStockFilters<TBuilder>(
        TBuilder query,
        StockFilters? filters,
        Func<TBuilder, Expression<Func<Stock, bool>>, TBuilder> where)
    {
        if (filters is null)
            return query;

        if (!string.IsNullOrWhiteSpace(filters.Search))
        {
            var term = filters.Search.Trim().ToLower();

            query = where(query, s =>
                s.Product.Sku.Contains(term) ||
                s.Product.Name.Contains(term) ||
                s.Product.Description.Contains(term));
        }

        if (filters.ProductId.HasValue)
            query = where(query, s =>
                s.ProductId == filters.ProductId.Value);

        switch (filters.AvailabilityFilter)
        {
            case StockAvailabilityFilter.InStock:
                query = where(query, s =>
                    s.Quantity > 0);
                break;

            case StockAvailabilityFilter.LowStock:
                query = where(query, s =>
                    s.Quantity > 0 &&
                    s.Quantity <= Stock.LowStockThreshold);
                break;

            case StockAvailabilityFilter.SufficientStock:
                query = where(query, s =>
                    s.Quantity > Stock.LowStockThreshold);
                break;

            case StockAvailabilityFilter.OutOfStock:
                query = where(query, s =>
                    s.Quantity == 0);
                break;

            case StockAvailabilityFilter.All:
                break;
        }

        return query;
    }
}