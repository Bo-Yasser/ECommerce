namespace ECommerce.UseCases.Features.Products.Models;
public sealed record ProductFilters(
    string? Search = null,
    Guid? BrandId = null,
    Guid? TypeId = null);
