using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Persistence.Configurations;
public sealed class ProductBrandConfiguration : IEntityTypeConfiguration<ProductBrand>
{
    public void Configure(EntityTypeBuilder<ProductBrand> builder)
    {
        BaseEntityConfiguration.Configure(builder);

        builder.Property(pb => pb.Name)
            .IsRequired()
            .HasMaxLength(ProductBrand.MaxNameLength);

        builder.Navigation(b => b.Products)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasIndex(pb => pb.Name)
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");
    }
}