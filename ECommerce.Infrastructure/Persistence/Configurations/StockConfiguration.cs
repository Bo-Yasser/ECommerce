using ECommerce.Domain.Entities.StockAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Persistence.Configurations;

public sealed class StockConfiguration : IEntityTypeConfiguration<Stock>
{
    public void Configure(EntityTypeBuilder<Stock> builder)
    {
        builder.ToTable("Stocks", t =>
        {
            t.HasCheckConstraint("CK_Stock_Quantity_NonNegative", "[Quantity] >= 0");
        });

        BaseEntityConfiguration.Configure(builder);

        builder.HasKey(s => s.Id);

        builder.Property(s => s.RowVersion)
            .IsRowVersion();

        builder.HasOne(s => s.Product)
            .WithOne(p => p.Stock)
            .HasForeignKey<Stock>(s => s.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(s => s.ProductId)
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        builder.Navigation(s => s.Transactions)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}