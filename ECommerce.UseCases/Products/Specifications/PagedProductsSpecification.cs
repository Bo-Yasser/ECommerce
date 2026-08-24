using ECommerce.Domain.Entities;
using ECommerce.UseCases.Products.Enums;
using ECommerce.UseCases.Products.Responses;
using ECommerce.UseCases.Specifications;

namespace ECommerce.UseCases.Products.Specifications;

public class PagedProductsSpecification : Specification<Product, GetProductsResponse>
{
    public PagedProductsSpecification(
        string? search = null,
        Guid? brandId = null,
        Guid? typeId = null,
        ProductSortField? sortBy = null,
        bool sortDescending = false,
        int? pageNumber = null,
        int? pageSize = null)
    {
        var query = Query;
        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query.Where(p => p.Name.Contains(term) || p.Description.Contains(term));
        }

        if (brandId.HasValue)
        {
            query.Where(p => p.ProductBrandId == brandId.Value);
        }

        if (typeId.HasValue)
        {
            query.Where(p => p.ProductTypeId == typeId.Value);
        }

        if(sortBy is ProductSortField sortField)
        {
            ApplySort(query, sortField, sortDescending);
        }

        if(pageNumber.HasValue && pageSize.HasValue)
        {
            var skip = (pageNumber.Value - 1) * pageSize.Value;
            query
                .Skip(skip)
                .Take(pageSize.Value)
                .Select(p => new GetProductsResponse(
                        p.Id,
                        p.Name,
                        p.Description,
                        p.Price,
                        p.PictureUrl,
                        p.ProductType.Name,
                        p.ProductBrand.Name
                    ));
        }
    }

    private void ApplySort(
        ISpecificationBuilder<Product, GetProductsResponse> query,
        ProductSortField? sortBy,
        bool sortDescending)
    {
        switch (sortBy)
        {
            case ProductSortField.Name:
                if (sortDescending)
                {
                    query.OrderByDescending(p => p.Name);
                }
                else
                {
                    query.OrderBy(p => p.Name);
                }
                break;

            case ProductSortField.Price:
                if (sortDescending)
                {
                    query
                        .OrderByDescending(p => p.Price)
                        .ThenBy(p => p.Name);
                }
                else
                {
                    query
                        .OrderBy(p => p.Price)
                        .ThenBy(p => p.Name);
                }
                break;

            case ProductSortField.Brand:
                if (sortDescending)
                {
                    query
                        .OrderByDescending(p => p.ProductBrand.Name)
                        .ThenBy(p => p.Name);
                }
                else
                {
                    query
                        .OrderBy(p => p.ProductBrand.Name)
                        .ThenBy(p => p.Name);
                }
                break;

            case ProductSortField.Type:
                if (sortDescending)
                {
                    query
                        .OrderByDescending(p => p.ProductType.Name)
                        .ThenBy(p => p.Name);
                }
                else
                {
                    query
                        .OrderBy(p => p.ProductType.Name)
                        .ThenBy(p => p.Name);
                }
                break;
        }
    }
}
