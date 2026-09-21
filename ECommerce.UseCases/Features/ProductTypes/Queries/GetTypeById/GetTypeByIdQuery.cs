using ECommerce.Domain.Common;
using ECommerce.UseCases.Features.ProductTypes.Responses;
using MediatR;

namespace ECommerce.UseCases.Features.ProductTypes.Queries.GetTypeById;

public sealed record GetTypeByIdQuery(Guid Id) : IRequest<Result<TypeResponse>>;
