namespace ECommerce.API.Contracts.Requests.Products;

public sealed record UpdateProductRequest(
    string Sku,
    string Name,
    string Description,
    decimal Price,
    string PictureUrl,
    Guid ProductTypeId,
    Guid ProductBrandId,
    byte[] RowVersion);