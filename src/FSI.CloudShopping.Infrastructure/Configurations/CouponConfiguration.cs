namespace FSI.CloudShopping.Infrastructure.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FSI.CloudShopping.Domain.Entities;

public class CouponConfiguration : IEntityTypeConfiguration<Coupon>
{
    public void Configure(EntityTypeBuilder<Coupon> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Code)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(c => c.Description)
            .HasMaxLength(500);

        builder.Property(c => c.DiscountType)
            .IsRequired();

        builder.Property(c => c.DiscountValue)
            .HasPrecision(18, 2);

        builder.OwnsOne(c => c.MinOrderValue, m =>
        {
            m.Property(x => x.Amount)
                .HasColumnName("MinOrderValue")
                .HasPrecision(18, 2);

            m.Property(x => x.Currency)
                .HasColumnName("MinOrderValueCurrency")
                .HasMaxLength(3)
                .HasDefaultValue("BRL");
        });

        builder.Property(c => c.MaxUsages)
            .HasDefaultValue(0);

        builder.Property(c => c.CurrentUsages)
            .HasDefaultValue(0);

        builder.Property(c => c.ValidFrom)
            .IsRequired();

        builder.Property(c => c.ValidTo)
            .IsRequired();

        builder.Property(c => c.IsActive)
            .HasDefaultValue(true);

        // Indexes
        builder.HasIndex(c => c.Code)
            .IsUnique();

        builder.HasIndex(c => c.IsActive);
    }
}
