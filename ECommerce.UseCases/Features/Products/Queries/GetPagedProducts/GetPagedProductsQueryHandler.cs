using ECommerce.Domain.Common;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using ECommerce.UseCases.Common.Pagination;
using ECommerce.UseCases.Features.Products.Responses;
using ECommerce.UseCases.Features.Products.Specifications;
using MediatR;

namespace ECommerce.UseCases.Features.Products.Queries.GetPagedProducts;

public sealed class GetPagedProductsQueryHandler(IReadRepository<Product> repository)
    : IRequestHandler<GetPagedProductsQuery, Result<PagedResult<ProductResponse>>>
{
    public async Task<Result<PagedResult<ProductResponse>>> Handle(GetPagedProductsQuery request, CancellationToken cancellationToken)
    {
        var countSpec = new ProductsFilterSpecification(request.Filters);

        var listSpec = new PagedProductsSpecification(
                filters: request.Filters,
                sortBy: request.SortBy,
                sortDescending: request.SortDescending,
                pageNumber: request.PageNumber,
                pageSize: request.PageSize);

        var productsCount = await repository.CountAsync(countSpec, cancellationToken);
        var products = await repository.ListAsync(listSpec, cancellationToken);

        return Result<PagedResult<ProductResponse>>.Success(new PagedResult<ProductResponse>(products, productsCount));
    }
}
