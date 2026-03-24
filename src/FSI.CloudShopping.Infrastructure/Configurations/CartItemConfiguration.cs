namespace FSI.CloudShopping.Infrastructure.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FSI.CloudShopping.Domain.Entities;

public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> builder)
    {
        builder.HasKey(ci => ci.Id);

        builder.Property(ci => ci.CartId)
            .IsRequired();

        builder.Property(ci => ci.ProductId)
            .IsRequired();

        builder.Property(ci => ci.ProductName)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(ci => ci.ProductImageUrl)
            .HasMaxLength(500);

        builder.Property(ci => ci.ProductSku)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(ci => ci.Quantity)
            .IsRequired();

        builder.OwnsOne(ci => ci.UnitPrice, p =>
        {
            p.Property(x => x.Amount)
                .HasColumnName("UnitPrice")
                .HasPrecision(18, 2);

            p.Property(x => x.Currency)
                .HasColumnName("UnitPriceCurrency")
                .HasMaxLength(3)
                .HasDefaultValue("BRL");
        });

        builder.HasIndex(ci => ci.CartId);
    }
}
