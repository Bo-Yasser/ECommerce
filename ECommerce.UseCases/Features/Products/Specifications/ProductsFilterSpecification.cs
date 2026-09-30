using ECommerce.Domain.Entities;
using ECommerce.UseCases.Features.Products.Models;
using ECommerce.UseCases.Specifications;

namespace ECommerce.UseCases.Features.Products.Specifications;
public sealed class ProductsFilterSpecification : Specification<Product>
{
    public ProductsFilterSpecification(ProductFilters? filters = null)
        => Query.ApplyFilters(filters);
}
