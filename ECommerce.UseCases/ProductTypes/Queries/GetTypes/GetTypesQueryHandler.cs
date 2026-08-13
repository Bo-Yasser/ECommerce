using ECommerce.Domain.Common;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using ECommerce.UseCases.ProductTypes.Responses;
using ECommerce.UseCases.ProductTypes.Specifications;
using MediatR;

namespace ECommerce.UseCases.ProductTypes.Queries.GetTypes;

public class GetTypesQueryHandler(IReadRepository<ProductType> repository) : IRequestHandler<GetTypesQuery, Result<IReadOnlyList<GetTypesResponse>>>
{
    public async Task<Result<IReadOnlyList<GetTypesResponse>>> Handle(GetTypesQuery request, CancellationToken cancellationToken)
    {
        var types = await repository.ListAsync(new TypesListSpecification(), cancellationToken);
        return Result<IReadOnlyList<GetTypesResponse>>.Success(types);
    }
}
