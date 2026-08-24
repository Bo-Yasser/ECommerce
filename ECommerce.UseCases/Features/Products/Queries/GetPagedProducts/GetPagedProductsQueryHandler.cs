using ECommerce.Domain.Common;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using ECommerce.UseCases.Common.Pagination;
using ECommerce.UseCases.Features.Products.Responses;
using ECommerce.UseCases.Features.Products.Specifications;
using MediatR;

namespace ECommerce.UseCases.Features.Products.Queries.GetPagedProducts;

public class GetPagedProductsQueryHandler(IReadRepository<Product> repository)
    : IRequestHandler<GetPagedProductsQuery, Result<PagedResult<GetProductsResponse>>>
{
    public async Task<Result<PagedResult<GetProductsResponse>>> Handle(GetPagedProductsQuery request, CancellationToken cancellationToken)
    {
        var countSpec = new PagedProductsSpecification(
                request.Search,
                request.BrandId,
                request.TypeId);

        var listSpecification = new PagedProductsSpecification(
                search: request.Search,
                brandId: request.BrandId,
                typeId: request.TypeId,
                sortBy: request.SortBy,
                sortDescending: request.SortDescending,
                pageNumber: request.PageNumber,
                pageSize: request.PageSize);

        var productsCount = await repository.CountAsync(countSpec, cancellationToken);
        var products = await repository.ListAsync(listSpecification, cancellationToken);

        return Result<PagedResult<GetProductsResponse>>.Success(new PagedResult<GetProductsResponse>(products, productsCount));
    }
}
