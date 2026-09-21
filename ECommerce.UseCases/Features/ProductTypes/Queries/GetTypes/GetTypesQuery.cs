using ECommerce.Domain.Common;
using ECommerce.UseCases.Features.ProductTypes.Responses;
using MediatR;

namespace ECommerce.UseCases.Features.ProductTypes.Queries.GetTypes;

public sealed record GetTypesQuery(string? Search) : IRequest<Result<IReadOnlyList<TypeResponse>>>;
