using ECommerce.Infrastructure.Persistence.DbContexts;
using ECommerce.UseCases.ProductBrands;
using ECommerce.UseCases.ProductBrands.Dtos;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Persistence.Queries;

public class ProductBrandQueryService(StoreDbContext dbContext) : IProductBrandQueryService
{
    public async Task<IReadOnlyList<GetAllBrandsResponse>> GetAllBrandsAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Types
            .AsNoTracking()
            .ProjectToType<GetAllBrandsResponse>()
            .ToListAsync(cancellationToken);
    }
}
