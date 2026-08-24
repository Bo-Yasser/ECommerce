using ECommerce.Domain.Common;
using ECommerce.UseCases.Features.Products.Responses;
using MediatR;

namespace ECommerce.UseCases.Features.Products.Queries.GetProducts;

public sealed record GetProductsQuery : IRequest<Result<IReadOnlyList<GetProductsResponse>>>;
