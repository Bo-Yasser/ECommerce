using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Identity;

public sealed class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshTokens");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Token)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(r => r.Token)
            .IsUnique();

        builder.HasOne<ApplicationUser>()
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
