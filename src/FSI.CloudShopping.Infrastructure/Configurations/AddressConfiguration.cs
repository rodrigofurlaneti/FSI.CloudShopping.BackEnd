namespace FSI.CloudShopping.Infrastructure.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FSI.CloudShopping.Domain.Entities;

public class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.CustomerId)
            .IsRequired();

        builder.Property(a => a.AddressType)
            .IsRequired();

        builder.Property(a => a.Street)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(a => a.Number)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(a => a.Complement)
            .HasMaxLength(200);

        builder.Property(a => a.Neighborhood)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(a => a.City)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(a => a.State)
            .HasMaxLength(2)
            .IsRequired();

        builder.OwnsOne(a => a.ZipCode, z =>
        {
            z.Property(x => x.Value)
                .HasColumnName("ZipCode")
                .HasMaxLength(8)
                .IsRequired();
        });

        builder.Property(a => a.Country)
            .HasMaxLength(2)
            .HasDefaultValue("BR");

        builder.Property(a => a.IsDefault)
            .HasDefaultValue(false);

        builder.HasIndex(a => a.CustomerId);
    }
}
