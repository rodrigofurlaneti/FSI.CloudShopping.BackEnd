namespace FSI.CloudShopping.Infrastructure.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FSI.CloudShopping.Domain.Entities;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.OwnsOne(p => p.Sku, s =>
        {
            s.Property(x => x.Value)
                .HasColumnName("Sku")
                .HasMaxLength(50)
                .IsRequired();
        });

        builder.OwnsOne(p => p.Slug, sl =>
        {
            sl.Property(x => x.Value)
                .HasColumnName("Slug")
                .HasMaxLength(200)
                .IsRequired();
        });

        builder.Property(p => p.Description)
            .HasMaxLength(2000);

        builder.Property(p => p.ShortDescription)
            .HasMaxLength(500);

        builder.OwnsOne(p => p.Price, pr =>
        {
            pr.Property(x => x.Amount)
                .HasColumnName("Price")
                .HasPrecision(18, 2);

            pr.Property(x => x.Currency)
                .HasColumnName("PriceCurrency")
                .HasMaxLength(3)
                .HasDefaultValue("BRL");
        });

        builder.OwnsOne(p => p.CompareAtPrice, c =>
        {
            c.Property(x => x.Amount)
                .HasColumnName("CompareAtPrice")
                .HasPrecision(18, 2);

            c.Property(x => x.Currency)
                .HasColumnName("CompareAtPriceCurrency")
                .HasMaxLength(3)
                .HasDefaultValue("BRL");
        });

        builder.OwnsOne(p => p.CostPrice, cp =>
        {
            cp.Property(x => x.Amount)
                .HasColumnName("CostPrice")
                .HasPrecision(18, 2);

            cp.Property(x => x.Currency)
                .HasColumnName("CostPriceCurrency")
                .HasMaxLength(3)
                .HasDefaultValue("BRL");
        });

        builder.Property(p => p.StockQuantity);
        builder.Property(p => p.ReservedQuantity);
        builder.Property(p => p.MinStockAlert);
        builder.Property(p => p.CategoryId);
        builder.Property(p => p.Status);
        builder.Property(p => p.ImageUrl).HasMaxLength(500);
        builder.Property(p => p.IsActive).HasDefaultValue(true);
        builder.Property(p => p.IsFeatured).HasDefaultValue(false);
        builder.Property(p => p.Weight);

        // Relationships
        builder.HasOne(p => p.Category)
            .WithMany()
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(p => p.Images)
            .WithOne(pi => pi.Product)
            .HasForeignKey(pi => pi.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(p => p.Sku)
            .IsUnique();

        builder.HasIndex(p => p.Slug)
            .IsUnique();

        builder.HasIndex(p => p.CategoryId);
        builder.HasIndex(p => p.IsActive);
        builder.HasIndex(p => p.IsFeatured);
    }
}
