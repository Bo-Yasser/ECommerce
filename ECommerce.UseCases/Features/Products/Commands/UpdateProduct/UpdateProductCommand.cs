using ECommerce.Domain.Common;
using MediatR;

namespace ECommerce.UseCases.Features.Products.Commands.UpdateProduct;
public sealed record UpdateProductCommand(
    Guid Id,
    string Sku,
    string Name,
    string Description,
    decimal Price,
    string PictureUrl,
    Guid ProductTypeId,
    Guid ProductBrandId,
    byte[] RowVersion) : IRequest<Result>;