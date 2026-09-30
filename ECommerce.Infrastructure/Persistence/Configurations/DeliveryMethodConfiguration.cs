using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Persistence.Configurations;

public sealed class DeliveryMethodConfiguration : IEntityTypeConfiguration<DeliveryMethod>
{
    public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
    {
        builder.ToTable("DeliveryMethods", t =>
        {
            t.HasCheckConstraint("CK_DeliveryMethods_Price", "[Price] >= 0");
            t.HasCheckConstraint("CK_DeliveryMethods_DisplayOrder", "[DisplayOrder] >= 0");
        });

        BaseEntityConfiguration.Configure(builder);

        builder.Property(dm => dm.Name)
            .HasMaxLength(DeliveryMethod.MaxNameLength)
            .IsRequired();

        builder.Property(dm => dm.Description)
            .HasMaxLength(DeliveryMethod.MaxDescriptionLength)
            .IsRequired(false);

        builder.Property(dm => dm.EstimatedDeliveryTime)
            .HasMaxLength(DeliveryMethod.MaxDeliveryTimeLength)
            .IsRequired();

        builder.Property(dm => dm.Price)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(dm => dm.IsAvailable)
            .HasDefaultValue(true);

        builder.Property(dm => dm.DisplayOrder)
            .HasDefaultValue(0);

        builder.Property(dm => dm.RowVersion)
            .IsRowVersion();

        builder.HasIndex(dm => dm.Name)
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        builder.HasIndex(dm => new { dm.IsAvailable, dm.DisplayOrder })
            .HasFilter("[IsDeleted] = 0");
    }
}