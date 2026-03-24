namespace FSI.CloudShopping.Infrastructure.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FSI.CloudShopping.Domain.Entities;

public class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.CustomerId)
            .IsRequired();

        builder.OwnsOne(c => c.BusinessTaxId, b =>
        {
            b.Property(x => x.Value)
                .HasColumnName("BusinessTaxId")
                .HasMaxLength(14)
                .IsRequired();
        });

        builder.Property(c => c.CompanyName)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(c => c.StateTaxId)
            .HasMaxLength(50);

        builder.Property(c => c.TradeName)
            .HasMaxLength(200);

        builder.HasIndex(c => c.CustomerId)
            .IsUnique();
    }
}
