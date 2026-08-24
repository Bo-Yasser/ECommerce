namespace ECommerce.UseCases.Features.Products.Responses;

public record GetProductsResponse(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    string PictureUrl,
    string ProductType,
    string ProductBrand
);