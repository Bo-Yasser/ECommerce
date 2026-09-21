using ECommerce.Domain.Common;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using ECommerce.UseCases.Features.ProductTypes.Responses;
using ECommerce.UseCases.Features.ProductTypes.Specifications;
using MediatR;

namespace ECommerce.UseCases.Features.ProductTypes.Queries.GetTypes;

public sealed class GetTypesQueryHandler(IReadRepository<ProductType> repository) : IRequestHandler<GetTypesQuery, Result<IReadOnlyList<TypeResponse>>>
{
    public async Task<Result<IReadOnlyList<TypeResponse>>> Handle(GetTypesQuery request, CancellationToken cancellationToken)
    {
        var types = await repository.ListAsync(
            new TypesListSpecification(request.Search),
            cancellationToken);

        return Result<IReadOnlyList<TypeResponse>>.Success(types);
    }
}
