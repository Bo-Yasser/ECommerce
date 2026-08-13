using ECommerce.Domain.Common;
using ECommerce.UseCases.Products.Responses;
using MediatR;

namespace ECommerce.UseCases.Products.Queries.GetProductById;

public sealed record GetProductByIdQuery(Guid Id) : IRequest<Result<GetProductByIdResponse>>;