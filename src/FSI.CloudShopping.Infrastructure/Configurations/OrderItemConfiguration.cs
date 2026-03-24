namespace FSI.CloudShopping.Infrastructure.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FSI.CloudShopping.Domain.Entities;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(oi => oi.Id);

        builder.Property(oi => oi.OrderId)
            .IsRequired();

        builder.Property(oi => oi.ProductId)
            .IsRequired();

        builder.Property(oi => oi.ProductName)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(oi => oi.ProductSku)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(oi => oi.Quantity)
            .IsRequired();

        builder.OwnsOne(oi => oi.UnitPrice, p =>
        {
            p.Property(x => x.Amount)
                .HasColumnName("UnitPrice")
                .HasPrecision(18, 2);

            p.Property(x => x.Currency)
                .HasColumnName("UnitPriceCurrency")
                .HasMaxLength(3)
                .HasDefaultValue("BRL");
        });

        builder.OwnsOne(oi => oi.Discount, d =>
        {
            d.Property(x => x.Amount)
                .HasColumnName("Discount")
                .HasPrecision(18, 2)
                .HasDefaultValue(0);

            d.Property(x => x.Currency)
                .HasColumnName("DiscountCurrency")
                .HasMaxLength(3)
                .HasDefaultValue("BRL");
        });

        builder.HasIndex(oi => oi.OrderId);
    }
}
