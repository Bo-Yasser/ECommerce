using ECommerce.Domain.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Persistence.Configurations;

public sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders", t =>
        {
            t.HasCheckConstraint("CK_Orders_SubTotal", "[SubTotal] >= 0");
            t.HasCheckConstraint("CK_Orders_ShippingCost", "[ShippingCost] >= 0");
            t.HasCheckConstraint("CK_Orders_Total", "[Total] >= 0");
            t.HasCheckConstraint("CK_Orders_DeliveryMethodPrice", "[DeliveryMethodPrice] >= 0");
        });

        BaseEntityConfiguration.Configure(builder);

        builder.Property(o => o.UserId)
            .IsRequired();

        builder.Property(o => o.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(o => o.SubTotal)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(o => o.ShippingCost)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(o => o.Total)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.ComplexProperty(o => o.DeliveryMethod, dm =>
        {
            dm.Property(d => d.DeliveryMethodName)
                .HasColumnName("DeliveryMethodName")
                .HasMaxLength(OrderDeliveryMethod.MaxNameLength)
                .IsRequired();

            dm.Property(d => d.DeliveryMethodPrice)
                .HasColumnName("DeliveryMethodPrice")
                .HasPrecision(18, 2)
                .IsRequired();

            dm.Property(d => d.DeliveryMethodEstimatedTime)
                .HasColumnName("DeliveryMethodEstimatedTime")
                .HasMaxLength(OrderDeliveryMethod.MaxEstimatedTimeLength)
                .IsRequired();
        });

        builder.OwnsOne(o => o.ShippingAddress, sa =>
        {
            sa.Property(a => a.RecipientFirstName)
                .HasColumnName("ShippingRecipientFirstName")
                .HasMaxLength(ShippingAddress.MaxNameLength)
                .IsRequired();

            sa.Property(a => a.RecipientLastName)
                .HasColumnName("ShippingRecipientLastName")
                .HasMaxLength(ShippingAddress.MaxNameLength)
                .IsRequired();

            sa.Property(a => a.PhoneNumber)
                .HasColumnName("ShippingPhoneNumber")
                .HasMaxLength(ShippingAddress.MaxPhoneLength)
                .IsRequired();

            sa.Property(a => a.Country)
                .HasColumnName("ShippingCountry")
                .HasMaxLength(ShippingAddress.MaxCountryLength)
                .IsRequired();

            sa.Property(a => a.City)
                .HasColumnName("ShippingCity")
                .HasMaxLength(ShippingAddress.MaxCityLength)
                .IsRequired();

            sa.Property(a => a.Street)
                .HasColumnName("ShippingStreet")
                .HasMaxLength(ShippingAddress.MaxStreetLength)
                .IsRequired();

            sa.Property(a => a.PostalCode)
                .HasColumnName("ShippingPostalCode")
                .HasMaxLength(ShippingAddress.MaxPostalCodeLength)
                .IsRequired();

            sa.HasIndex(a => a.PhoneNumber)
                .HasFilter("[IsDeleted] = 0");
        });

        builder.Navigation(o => o.Items)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasIndex(o => o.Status)
            .HasFilter("[IsDeleted] = 0");

        builder.HasIndex(o => o.DeliveryMethodId)
            .HasFilter("[IsDeleted] = 0");

        builder.HasIndex(o => new { o.UserId, o.Status })
            .HasFilter("[IsDeleted] = 0");

        builder.HasIndex(o => new { o.UserId, o.CreatedAt })
            .HasFilter("[IsDeleted] = 0");
    }
}