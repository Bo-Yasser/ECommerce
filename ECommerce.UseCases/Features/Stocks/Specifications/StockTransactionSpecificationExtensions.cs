using ECommerce.Domain.Entities.StockAggregate;
using ECommerce.UseCases.Features.Stocks.Models;
using ECommerce.UseCases.Specifications;
using System.Linq.Expressions;


namespace ECommerce.UseCases.Features.Stocks.Specifications;

public static class StockTransactionSpecificationExtensions
{
    public static ISpecificationBuilder<StockTransaction> ApplyFilters(
        this ISpecificationBuilder<StockTransaction> query,
        StockTransactionFilters? filters)
    {
        return ApplyStockTransactionFilters(
            query,
            filters,
            static (q, predicate) => q.Where(predicate));
    }

    public static ISpecificationBuilder<StockTransaction, TResult> ApplyFilters<TResult>(
        this ISpecificationBuilder<StockTransaction, TResult> query,
        StockTransactionFilters? filters)
    {
        return ApplyStockTransactionFilters(
            query,
            filters,
            static (q, predicate) => q.Where(predicate));
    }

    private static TBuilder ApplyStockTransactionFilters<TBuilder>(
        TBuilder query,
        StockTransactionFilters? filters,
        Func<TBuilder, Expression<Func<StockTransaction, bool>>, TBuilder> where)
    {
        if (filters is null)
            return query;

        if (!string.IsNullOrWhiteSpace(filters.Search))
        {
            var term = filters.Search.Trim().ToLower();

            query = where(query, st =>
                st.Type.ToString().Contains(term) ||
                (st.Notes != null && st.Notes.Contains(term)));
        }

        if (filters.ReferenceId.HasValue)
            query = where(query, st =>
                st.ReferenceId == filters.ReferenceId.Value);

        return query;
    }
}