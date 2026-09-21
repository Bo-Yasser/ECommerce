using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Persistence.Configurations;


public sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products", t =>
        {
            t.HasCheckConstraint("CK_Products_Price", "Price >= 0");
        });

        BaseEntityConfiguration.Configure(builder);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(Product.MaxNameLength);

        builder.Property(p => p.Description)
            .IsRequired()
            .HasMaxLength(Product.MaxDescriptionLength);

        builder.Property(p => p.PictureUrl)
            .IsRequired()
            .HasMaxLength(Product.MaxPictureUrlLength);

        builder.Property(p => p.Price)
            .HasPrecision(18, 2);

        builder.Property(p => p.Sku)
           .HasMaxLength(Product.MaxSkuLength)
           .IsRequired();

        builder.HasOne(p => p.ProductBrand)
            .WithMany(pb => pb.Products)
            .HasForeignKey(p => p.ProductBrandId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(p => p.ProductType)
            .WithMany(pt => pt.Products)
            .HasForeignKey(p => p.ProductTypeId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasIndex(p => p.Name)
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        builder.HasIndex(p => p.Price)
            .HasFilter("[IsDeleted] = 0");

        // Override Foreign Key default Indexes to Filtered Indexes
        builder.HasIndex(p => p.ProductBrandId)
             .HasFilter("[IsDeleted] = 0");

        builder.HasIndex(p => p.ProductTypeId)
            .HasFilter("[IsDeleted] = 0");

        builder.HasIndex(p => p.Sku)
            .HasFilter("[IsDeleted] = 0")
            .IsUnique();

    }
}
