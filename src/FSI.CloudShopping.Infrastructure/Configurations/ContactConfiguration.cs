namespace FSI.CloudShopping.Infrastructure.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FSI.CloudShopping.Domain.Entities;

public class ContactConfiguration : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.CustomerId)
            .IsRequired();

        builder.Property(c => c.Name)
            .HasMaxLength(150)
            .IsRequired();

        builder.OwnsOne(c => c.Email, e =>
        {
            e.Property(x => x.Value)
                .HasColumnName("Email")
                .HasMaxLength(254)
                .IsRequired();
        });

        builder.OwnsOne(c => c.Phone, p =>
        {
            p.Property(x => x.Value)
                .HasColumnName("Phone")
                .HasMaxLength(11);
        });

        builder.Property(c => c.Position)
            .HasMaxLength(100);

        builder.Property(c => c.IsPrimary)
            .HasDefaultValue(false);

        builder.HasIndex(c => c.CustomerId);
    }
}
