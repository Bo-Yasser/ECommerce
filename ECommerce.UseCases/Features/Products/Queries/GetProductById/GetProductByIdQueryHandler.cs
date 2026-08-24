using ECommerce.Domain.Common;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using ECommerce.UseCases.Features.Products.Specifications;
using MediatR;
using ECommerce.Domain.Common.Errors;
using ECommerce.UseCases.Features.Products.Responses;

namespace ECommerce.UseCases.Features.Products.Queries.GetProductById;

public class GetProductByIdQueryHandler(IReadRepository<Product> repository) : IRequestHandler<GetProductByIdQuery, Result<GetProductByIdResponse>>
{
    public async Task<Result<GetProductByIdResponse>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await repository.FirstOrDefaultAsync(new ProductByIdSpecification(request.Id), cancellationToken);
        if(product is null)
        {
            return Result<GetProductByIdResponse>.Failure(ProductErrors.NotFound);
        }
        return Result<GetProductByIdResponse>.Success(product);
    }
}