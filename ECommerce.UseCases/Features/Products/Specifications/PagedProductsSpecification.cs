using ECommerce.Domain.Entities;
using ECommerce.UseCases.Features.Products.Enums;
using ECommerce.UseCases.Features.Products.Models;
using ECommerce.UseCases.Features.Products.Responses;
using ECommerce.UseCases.Specifications;

namespace ECommerce.UseCases.Features.Products.Specifications;

public sealed class PagedProductsSpecification : Specification<Product, ProductResponse>
{
    public PagedProductsSpecification(
        ProductFilters? filters = null,
        ProductSortField sortBy = ProductSortField.Name,
        bool sortDescending = false,
        int? pageNumber = null,
        int? pageSize = null)
    {
        var query = Query;

        query = query.ApplyFilters(filters);

        ApplySort(query, sortBy, sortDescending);

        if (pageNumber.HasValue && pageSize.HasValue)
        {
            var skip = (pageNumber.Value - 1) * pageSize.Value;
            query
                .Skip(skip)
                .Take(pageSize.Value);
        }

        query.Select(p => new ProductResponse(
            p.Id,
            p.Sku,
            p.Name,
            p.Description,
            p.Price,
            p.PictureUrl,
            p.ProductType.Name,
            p.ProductBrand.Name,
            p.RowVersion,
            p.Stock.Quantity,
            p.Stock.Quantity > 0
        ));
    }

    private void ApplySort(
        ISpecificationBuilder<Product, ProductResponse> query,
        ProductSortField sortBy,
        bool sortDescending)
    {
        switch (sortBy)
        {
            case ProductSortField.Name:
                if (sortDescending)
                    query.OrderByDescending(p => p.Name)
                        .OrderByDescending(p => p.CreatedAt)
                        .ThenByDescending(p => p.Id);
                else
                    query.OrderBy(p => p.Name)
                        .ThenBy(p => p.CreatedAt)
                        .ThenBy(p => p.Id);
                break;

            case ProductSortField.Price:
                if (sortDescending)
                    query.OrderByDescending(p => p.Price)
                        .OrderByDescending(p => p.CreatedAt)
                        .ThenByDescending(p => p.Id);
                else
                    query.OrderBy(p => p.Price)
                        .ThenBy(p => p.CreatedAt)
                        .ThenBy(p => p.Id);
                break;

            case ProductSortField.Brand:
                if (sortDescending)
                    query.OrderByDescending(p => p.ProductBrand.Name)
                        .OrderByDescending(p => p.CreatedAt)
                        .ThenByDescending(p => p.Id);
                else
                    query.OrderBy(p => p.ProductBrand.Name)
                        .ThenBy(p => p.CreatedAt)
                        .ThenBy(p => p.Id);
                break;

            case ProductSortField.Type:
                if (sortDescending)
                    query.OrderByDescending(p => p.ProductType.Name)
                        .OrderByDescending(p => p.CreatedAt)
                        .ThenByDescending(p => p.Id);
                else
                    query.OrderBy(p => p.ProductType.Name)
                        .ThenBy(p => p.CreatedAt)
                        .ThenBy(p => p.Id);
                break;

            case ProductSortField.CreatedAt:
                if (sortDescending)
                    query.OrderByDescending(p => p.CreatedAt)
                        .ThenByDescending(p => p.Id);
                else
                    query.OrderBy(p => p.CreatedAt)
                        .ThenBy(p => p.Id);
                break;
        }
    }
}