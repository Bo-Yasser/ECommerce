using ECommerce.Domain.Common;
using ECommerce.UseCases.ProductTypes.Responses;
using MediatR;

namespace ECommerce.UseCases.ProductTypes.Queries.GetTypes;

public sealed record GetTypesQuery : IRequest<Result<IReadOnlyList<GetTypesResponse>>>;
