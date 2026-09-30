using ECommerce.UseCases.Features.Products.Models;
using ECommerce.UseCases.Specifications;
using System.Linq.Expressions;
using ECommerce.Domain.Entities;

namespace ECommerce.UseCases.Features.Products.Specifications;


public static class ProductSpecificationExtensions
{
    public static ISpecificationBuilder<Product> ApplyFilters(
        this ISpecificationBuilder<Product> query,
        ProductFilters? filters)
    {
        return ApplyProductFilters(
            query,
            filters,
            static (q, predicate) => q.Where(predicate));
    }

    public static ISpecificationBuilder<Product, TResult> ApplyFilters<TResult>(
        this ISpecificationBuilder<Product, TResult> query,
        ProductFilters? filters)
    {
        return ApplyProductFilters(
            query,
            filters,
            static (q, predicate) => q.Where(predicate));
    }

    private static TBuilder ApplyProductFilters<TBuilder>(
        TBuilder query,
        ProductFilters? filters,
        Func<TBuilder, Expression<Func<Product, bool>>, TBuilder> where)
    {
        if (filters is null)
            return query;

        if (!string.IsNullOrWhiteSpace(filters.Search))
        {
            var term = filters.Search.Trim().ToLower();

            query = where(query, p =>
                p.Name.Contains(term) ||
                p.Sku.Contains(term) ||
                p.Description.Contains(term) ||
                p.ProductBrand.Name.Contains(term) ||
                p.ProductType.Name.Contains(term));
        }

        if (filters.BrandId.HasValue)
            query = where(query, p => p.ProductBrandId == filters.BrandId.Value);

        if (filters.TypeId.HasValue)
            query = where(query, p => p.ProductTypeId == filters.TypeId.Value);

        return query;
    }
}