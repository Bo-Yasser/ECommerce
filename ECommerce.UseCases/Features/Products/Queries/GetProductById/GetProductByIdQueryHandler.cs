using ECommerce.Domain.Common;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using ECommerce.UseCases.Features.Products.Specifications;
using MediatR;
using ECommerce.Domain.Common.Errors;
using ECommerce.UseCases.Features.Products.Responses;

namespace ECommerce.UseCases.Features.Products.Queries.GetProductById;

public sealed class GetProductByIdQueryHandler(IReadRepository<Product> repository) : IRequestHandler<GetProductByIdQuery, Result<ProductResponse>>
{
    public async Task<Result<ProductResponse>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await repository.FirstOrDefaultAsync(new ProductByIdToResponseSpecification(request.Id), cancellationToken);
        if(product is null)
        {
            return Result<ProductResponse>.Failure(ProductErrors.NotFound);
        }
        return Result<ProductResponse>.Success(product);
    }
}