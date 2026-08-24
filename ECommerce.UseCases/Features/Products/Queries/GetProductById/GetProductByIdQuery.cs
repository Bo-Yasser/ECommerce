using ECommerce.Domain.Common;
using ECommerce.UseCases.Features.Products.Responses;
using MediatR;

namespace ECommerce.UseCases.Features.Products.Queries.GetProductById;

public sealed record GetProductByIdQuery(Guid Id) : IRequest<Result<GetProductByIdResponse>>;