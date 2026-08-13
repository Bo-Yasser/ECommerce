using ECommerce.Domain.Common;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using ECommerce.UseCases.Products.Responses;
using ECommerce.UseCases.Products.Specifications;
using MediatR;

namespace ECommerce.UseCases.Products.Queries.GetProducts;

public class GetProductsQueryHandler(IReadRepository<Product> repository) : IRequestHandler<GetProductsQuery, Result<IReadOnlyList<GetProductsResponse>>>
{
    public async Task<Result<IReadOnlyList<GetProductsResponse>>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await repository.ListAsync(new ProductsListSpecification(), cancellationToken);
        return Result<IReadOnlyList<GetProductsResponse>>.Success(products);
    }
}
