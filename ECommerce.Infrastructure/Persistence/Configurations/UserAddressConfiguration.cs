using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Persistence.Configurations;

public sealed class UserAddressConfiguration : IEntityTypeConfiguration<UserAddress>
{
    public void Configure(EntityTypeBuilder<UserAddress> builder)
    {
        builder.ToTable("UserAddresses");

        BaseEntityConfiguration.Configure(builder);

        builder.Property(ua => ua.RecipientFirstName)
            .HasMaxLength(UserAddress.MaxNameLength)
            .IsRequired();

        builder.Property(ua => ua.RecipientLastName)
            .HasMaxLength(UserAddress.MaxNameLength)
            .IsRequired();

        builder.Property(ua => ua.PhoneNumber)
            .HasMaxLength(UserAddress.MaxPhoneLength)
            .IsRequired();

        builder.Property(ua => ua.Country)
            .HasMaxLength(UserAddress.MaxCountryLength)
            .IsRequired();

        builder.Property(ua => ua.City)
            .HasMaxLength(UserAddress.MaxCityLength)
            .IsRequired();

        builder.Property(ua => ua.Street)
            .HasMaxLength(UserAddress.MaxStreetLength)
            .IsRequired();

        builder.Property(ua => ua.PostalCode)
            .HasMaxLength(UserAddress.MaxPostalCodeLength)
            .IsRequired();

        builder.Property(ua => ua.UserId)
            .IsRequired();
        builder.HasIndex(ua => ua.UserId);
    }
}
