using ECommerce.Domain.Entities.StockAggregate;
using ECommerce.UseCases.Features.Stocks.Models;
using ECommerce.UseCases.Specifications;

namespace ECommerce.UseCases.Features.Stocks.Specifications;
public sealed class StockTransactionsFilterSpecification
    : Specification<StockTransaction>
{
    public StockTransactionsFilterSpecification(Guid productId, StockTransactionFilters? filters)
        => Query.Where(st => st.Stock.ProductId == productId).ApplyFilters(filters);
}
