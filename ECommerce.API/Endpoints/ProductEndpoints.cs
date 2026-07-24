using ECommerce.UseCases.Products;
using ECommerce.UseCases.Products.Dtos;

namespace ECommerce.API.Endpoints;

public static class ProductEndpoints
{
    public static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/products").WithTags("Products");

        group.MapGet("/", async (IProductQueryService productQueryService, CancellationToken cancellationToken) =>
        {
            var products = await productQueryService.GetAllProductsAsync(cancellationToken);
            return Results.Ok(products);
        })
        .WithName("GetAllProducts")
        .Produces<IReadOnlyList<GetAllProductsResponse>>(StatusCodes.Status200OK);

        group.MapGet("/{id:guid}", async (Guid id, IProductQueryService productQueryService, CancellationToken cancellationToken) =>
        {
            var product = await productQueryService.GetByIdProductAsync(id, cancellationToken);
            return product is not null ? Results.Ok(product) : Results.NotFound();
        })
        .WithName("GetProductById")
        .Produces<GetByIdProductResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);


        return endpoints;
    }
}
