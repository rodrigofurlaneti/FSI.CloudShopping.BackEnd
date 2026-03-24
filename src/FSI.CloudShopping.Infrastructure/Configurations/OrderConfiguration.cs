namespace FSI.CloudShopping.Infrastructure.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FSI.CloudShopping.Domain.Entities;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);

        builder.Property(o => o.OrderNumber)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(o => o.CustomerId)
            .IsRequired();

        builder.Property(o => o.OrderDate)
            .IsRequired();

        builder.Property(o => o.Status)
            .IsRequired();

        builder.Property(o => o.ShippingAddressId)
            .IsRequired();

        builder.OwnsOne(o => o.SubTotal, s =>
        {
            s.Property(x => x.Amount)
                .HasColumnName("SubTotal")
                .HasPrecision(18, 2);

            s.Property(x => x.Currency)
                .HasColumnName("SubTotalCurrency")
                .HasMaxLength(3)
                .HasDefaultValue("BRL");
        });

        builder.OwnsOne(o => o.DiscountAmount, d =>
        {
            d.Property(x => x.Amount)
                .HasColumnName("DiscountAmount")
                .HasPrecision(18, 2)
                .HasDefaultValue(0);

            d.Property(x => x.Currency)
                .HasColumnName("DiscountAmountCurrency")
                .HasMaxLength(3)
                .HasDefaultValue("BRL");
        });

        builder.OwnsOne(o => o.ShippingCost, sc =>
        {
            sc.Property(x => x.Amount)
                .HasColumnName("ShippingCost")
                .HasPrecision(18, 2);

            sc.Property(x => x.Currency)
                .HasColumnName("ShippingCostCurrency")
                .HasMaxLength(3)
                .HasDefaultValue("BRL");
        });

        builder.OwnsOne(o => o.TotalAmount, t =>
        {
            t.Property(x => x.Amount)
                .HasColumnName("TotalAmount")
                .HasPrecision(18, 2);

            t.Property(x => x.Currency)
                .HasColumnName("TotalAmountCurrency")
                .HasMaxLength(3)
                .HasDefaultValue("BRL");
        });

        builder.Property(o => o.CouponCode)
            .HasMaxLength(50);

        builder.Property(o => o.TrackingNumber)
            .HasMaxLength(100);

        builder.Property(o => o.Notes)
            .HasMaxLength(1000);

        // Relationships
        builder.HasMany(o => o.Items)
            .WithOne(oi => oi.Order)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(o => o.ShippingAddress)
            .WithMany()
            .HasForeignKey(o => o.ShippingAddressId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(o => o.OrderNumber)
            .IsUnique();

        builder.HasIndex(o => o.CustomerId);
        builder.HasIndex(o => o.Status);
        builder.HasIndex(o => o.OrderDate);
    }
}
