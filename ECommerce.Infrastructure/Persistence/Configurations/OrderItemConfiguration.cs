using ECommerce.Domain.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Persistence.Configurations;

public sealed class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItems", t =>
        {
            t.HasCheckConstraint("CK_OrderItems_Quantity", "[Quantity] > 0");
            t.HasCheckConstraint("CK_OrderItems_UnitPrice", "[UnitPrice] >= 0");
        });

        BaseEntityConfiguration.Configure(builder);

        builder.Property(oi => oi.OrderId).IsRequired();
        builder.Property(oi => oi.ProductId).IsRequired();

        builder.OwnsOne(oi => oi.ItemOrdered, snapshot =>
        {
            snapshot.Property(p => p.Sku)
                .HasColumnName("ProductSku")
                .HasMaxLength(ProductItemOrdered.MaxSkuLength)
                .IsRequired();

            snapshot.Property(p => p.ProductName)
                .HasColumnName("ProductName")
                .HasMaxLength(ProductItemOrdered.MaxProductNameLength)
                .IsRequired();

            snapshot.Property(p => p.UnitPrice)
                .HasColumnName("UnitPrice")
                .HasPrecision(18, 2)
                .IsRequired();

            snapshot.Property(p => p.PictureUrl)
                .HasColumnName("PictureUrl")
                .HasMaxLength(ProductItemOrdered.MaxPictureUrlLength)
                .IsRequired();
        });

        builder.HasOne(oi => oi.Order)
            .WithMany(o => o.Items)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(oi => new { oi.OrderId, oi.ProductId })
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        builder.HasIndex(oi => oi.ProductId)
            .HasFilter("[IsDeleted] = 0");

    }
}