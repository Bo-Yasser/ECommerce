using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Errors;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using ECommerce.UseCases.Features.ProductTypes.Responses;
using ECommerce.UseCases.Features.ProductTypes.Specifications;
using MediatR;

namespace ECommerce.UseCases.Features.ProductTypes.Queries.GetTypeById;

public sealed class GetTypeByIdQueryHandler(
    IReadRepository<ProductType> repository)
    : IRequestHandler<GetTypeByIdQuery, Result<TypeResponse>>
{
    public async Task<Result<TypeResponse>> Handle(GetTypeByIdQuery request, CancellationToken cancellationToken)
    {
        var type = await repository.FirstOrDefaultAsync(
            new TypeByIdToResponseSpecification(request.Id),
            cancellationToken);

        if (type is null)
            return Result<TypeResponse>.Failure(TypeErrors.NotFound);

        return Result<TypeResponse>.Success(type);
    }
}
