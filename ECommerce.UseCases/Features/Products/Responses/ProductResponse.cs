namespace ECommerce.UseCases.Features.Products.Responses;

public sealed record ProductResponse(
    Guid Id,
    string Sku,
    string Name,
    string Description,
    decimal Price,
    string PictureUrl,
    string ProductType,
    string ProductBrand,
    byte[] RowVersion,
    int AvailableStock,
    bool InStock);