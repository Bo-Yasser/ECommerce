using ECommerce.Domain.Entities.StockAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Persistence.Configurations;

public sealed class StockTransactionConfiguration : IEntityTypeConfiguration<StockTransaction>
{
    public void Configure(EntityTypeBuilder<StockTransaction> builder)
    {
        builder.ToTable("StockTransactions", t =>
        {
            t.HasCheckConstraint("CK_Stock_Transaction_QuantityChanged_NonZero", "[QuantityChanged] != 0");
            t.HasCheckConstraint("CK_Stock_Transaction_QuantityBefore_NonNegative", "[QuantityBefore] >= 0");
            t.HasCheckConstraint("CK_Stock_Transaction_QuantityAfter_NonNegative", "[QuantityAfter] >= 0");
            t.HasCheckConstraint("CK_Stock_Transaction_QuantityMismatch", "[QuantityBefore] + [QuantityChanged] = [QuantityAfter]");
        });

        BaseEntityConfiguration.Configure(builder);

        builder.HasKey(st => st.Id);

        builder.Property(st => st.Notes)
            .HasMaxLength(500);

        builder.Property(st => st.Type)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.HasOne(st => st.Stock)
            .WithMany(s => s.Transactions)
            .HasForeignKey(st => st.StockId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(st => new { st.StockId, st.CreatedAt })
            .HasFilter("[IsDeleted] = 0");
    }
}