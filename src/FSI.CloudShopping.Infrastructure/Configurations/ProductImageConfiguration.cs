namespace FSI.CloudShopping.Infrastructure.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FSI.CloudShopping.Domain.Entities;

public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
{
    public void Configure(EntityTypeBuilder<ProductImage> builder)
    {
        builder.HasKey(pi => pi.Id);

        builder.Property(pi => pi.ProductId)
            .IsRequired();

        builder.Property(pi => pi.Url)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(pi => pi.AltText)
            .HasMaxLength(200);

        builder.Property(pi => pi.SortOrder)
            .HasDefaultValue(0);

        builder.Property(pi => pi.IsPrimary)
            .HasDefaultValue(false);

        builder.HasIndex(pi => pi.ProductId);
    }
}
