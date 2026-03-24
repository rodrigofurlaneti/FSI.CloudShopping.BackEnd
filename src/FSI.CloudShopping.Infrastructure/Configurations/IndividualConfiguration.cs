namespace FSI.CloudShopping.Infrastructure.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FSI.CloudShopping.Domain.Entities;

public class IndividualConfiguration : IEntityTypeConfiguration<Individual>
{
    public void Configure(EntityTypeBuilder<Individual> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.CustomerId)
            .IsRequired();

        builder.OwnsOne(i => i.TaxId, t =>
        {
            t.Property(x => x.Value)
                .HasColumnName("TaxId")
                .HasMaxLength(11)
                .IsRequired();
        });

        builder.OwnsOne(i => i.FullName, f =>
        {
            f.Property(x => x.FirstName)
                .HasColumnName("FirstName")
                .HasMaxLength(100)
                .IsRequired();

            f.Property(x => x.LastName)
                .HasColumnName("LastName")
                .HasMaxLength(100)
                .IsRequired();
        });

        builder.Property(i => i.BirthDate)
            .IsRequired();

        builder.HasIndex(i => i.CustomerId)
            .IsUnique();
    }
}
