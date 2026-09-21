using ECommerce.Domain.Common;
using MediatR;

namespace ECommerce.UseCases.Features.Products.Commands.CreateProduct;

public sealed record CreateProductCommand(
    string Sku,
    string Name,
    string Description,
    decimal Price,
    string PictureUrl,
    Guid ProductTypeId,
    Guid ProductBrandId) : IRequest<Result<Guid>>;