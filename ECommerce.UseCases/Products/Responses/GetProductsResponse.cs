namespace ECommerce.UseCases.Products.Responses;

public record GetProductsResponse(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    string PictureUrl,
    string ProductType,
    string ProductBrand
);