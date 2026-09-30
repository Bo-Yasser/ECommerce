using ECommerce.Domain.Entities.StockAggregate;
using ECommerce.UseCases.Features.Stocks.Enums;
using ECommerce.UseCases.Features.Stocks.Models;
using ECommerce.UseCases.Features.Stocks.Responses;
using ECommerce.UseCases.Specifications;

namespace ECommerce.UseCases.Features.Stocks.Specifications;

public sealed class PagedStockTransactionsByProductIdSpecification : Specification<StockTransaction, StockTransactionResponse>
{
    public PagedStockTransactionsByProductIdSpecification(
        Guid productId,
        StockTransactionFilters? filters,
        StockTransactionSortField sortBy = StockTransactionSortField.Reference,
        bool sortDescending = false,
        int? pageNumber = null,
        int? pageSize = null)
    {
        var query = Query;

        query.Where(st => st.Stock.ProductId == productId);

        query = query.ApplyFilters(filters);

        ApplySort(query, sortBy, sortDescending);

        if(pageNumber.HasValue && pageSize.HasValue)
        {
            var skip = (pageNumber.Value - 1) * pageSize.Value;
            query
                .Skip(skip)
                .Take(pageSize.Value);
        }

        query.Select(st => new StockTransactionResponse(
            st.Id,
            st.StockId,
            st.QuantityChanged,
            st.QuantityBefore,
            st.QuantityAfter,
            st.Type.ToString(),
            st.ReferenceId,
            st.Notes));
    }

    private void ApplySort(
        ISpecificationBuilder<StockTransaction, StockTransactionResponse> query,
        StockTransactionSortField sortBy,
        bool sortDescending)
    {
        switch (sortBy)
        {
            case StockTransactionSortField.Type:
                if (sortDescending)
                {
                    query
                        .OrderByDescending(st => st.Type)
                        .ThenByDescending(st => st.CreatedAt)
                        .ThenByDescending(st => st.Id);
                }
                else
                {
                    query
                        .OrderBy(st => st.Type)
                        .ThenBy(st => st.CreatedAt)
                        .ThenBy(st => st.Id);
                }

                break;

            case StockTransactionSortField.Reference:
                if (sortDescending)
                {
                    query
                        .OrderByDescending(st => st.ReferenceId)
                        .ThenByDescending(st => st.CreatedAt)
                        .ThenByDescending(st => st.Id);
                }
                else
                {
                    query
                        .OrderBy(st => st.ReferenceId)
                        .ThenBy(st => st.CreatedAt)
                        .ThenBy(st => st.Id);
                }

                break;

            case StockTransactionSortField.CreatedAt:
                if (sortDescending)
                {
                    query
                        .OrderByDescending(st => st.CreatedAt)
                        .ThenByDescending(st => st.Id);
                }
                else
                {
                    query
                        .OrderBy(st => st.CreatedAt)
                        .ThenBy(st => st.Id);
                }

                break;
        }
    }
}
