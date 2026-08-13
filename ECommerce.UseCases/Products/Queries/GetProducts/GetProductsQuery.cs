using ECommerce.Domain.Common;
using ECommerce.UseCases.Products.Responses;
using MediatR;

namespace ECommerce.UseCases.Products.Queries.GetProducts;

public sealed record GetProductsQuery : IRequest<Result<IReadOnlyList<GetProductsResponse>>>;
