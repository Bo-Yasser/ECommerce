using ECommerce.Domain.Common;
using ECommerce.UseCases.Common.Pagination;
using ECommerce.UseCases.Products.Enums;
using ECommerce.UseCases.Products.Responses;
using MediatR;

namespace ECommerce.UseCases.Products.Queries.GetPagedProducts;

public sealed record GetPagedProductsQuery(
        int PageNumber = 1,
        int PageSize = 5,
        string? Search = null,
        Guid? BrandId = null,
        Guid? TypeId = null,
        ProductSortField SortBy = ProductSortField.Name,
        bool SortDescending = false
    ) : IRequest<Result<PagedResult<GetProductsResponse>>>;