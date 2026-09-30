using ECommerce.Domain.Entities.StockAggregate;
using ECommerce.UseCases.Features.Stocks.Enums;
using ECommerce.UseCases.Features.Stocks.Models;
using ECommerce.UseCases.Specifications;

namespace ECommerce.UseCases.Features.Stocks.Specifications;

public sealed class StocksFilterSpecification : Specification<Stock>
{
    public StocksFilterSpecification(StockFilters? filters = null)
        => Query.ApplyFilters(filters);
}