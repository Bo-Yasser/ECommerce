using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Persistence.Seeding;

public sealed class DeliveryMethodSeeder(StoreDbContext dbContext) : IDataSeeder
{
    public int Order => 3;
    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        if (await dbContext.DeliveryMethods.AnyAsync(cancellationToken))
            return;

        var deliveryMethods = new[]
        {
            DeliveryMethod.Create(
                id: Guid.Parse("11111111-1111-1111-1111-111111111111"),
                name: "Standard Delivery",
                price: 5.00m,
                estimatedDeliveryTime:"3-5 business days",
                description: "Affordable ground shipping",
                displayOrder: 1).Value,

            DeliveryMethod.Create(
                id: Guid.Parse("22222222-2222-2222-2222-222222222222"),
                name: "Express Delivery",
                price: 15.00m,
                estimatedDeliveryTime:"1-2 business days",
                description: "Fast priority shipping",
                displayOrder: 2).Value,

            DeliveryMethod.Create(
                id: Guid.Parse("33333333-3333-3333-3333-333333333333"),
                name: "Same Day Delivery",
                price: 30.00m,
                estimatedDeliveryTime:"Same day",
                description: "Order before noon for same-day delivery",
                displayOrder: 3).Value,

        };

        await dbContext.DeliveryMethods.AddRangeAsync(deliveryMethods, cancellationToken);
    }
}
