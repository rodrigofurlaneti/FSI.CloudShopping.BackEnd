namespace FSI.CloudShopping.Infrastructure.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FSI.CloudShopping.Domain.Entities;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Email)
            .HasMaxLength(254)
            .IsRequired();

        builder.Property(c => c.SessionToken)
            .IsRequired();

        builder.Property(c => c.IsActive)
            .HasDefaultValue(true);

        builder.Property(c => c.CreatedAt)
            .HasDefaultValue(DateTime.UtcNow);

        builder.Property(c => c.UpdatedAt)
            .HasDefaultValue(DateTime.UtcNow);

        // Value object configurations
        builder.OwnsOne(c => c.Email, e =>
        {
            e.Property(x => x.Value)
                .HasColumnName("Email")
                .HasMaxLength(254)
                .IsRequired();
        });

        builder.OwnsOne(c => c.Password, p =>
        {
            p.Property(x => x.Hash)
                .HasColumnName("PasswordHash")
                .HasMaxLength(255);

            p.Property(x => x.Salt)
                .HasColumnName("PasswordSalt")
                .HasMaxLength(255);
        });

        // Relationships
        builder.HasOne(c => c.Individual)
            .WithOne(i => i.Customer)
            .HasForeignKey<Individual>(i => i.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(c => c.Company)
            .WithOne(co => co.Customer)
            .HasForeignKey<Company>(co => co.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.Addresses)
            .WithOne(a => a.Customer)
            .HasForeignKey(a => a.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.Contacts)
            .WithOne(co => co.Customer)
            .HasForeignKey(co => co.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(c => c.Email)
            .IsUnique();

        builder.HasIndex(c => c.SessionToken)
            .IsUnique();

        builder.HasIndex(c => c.RefreshToken)
            .IsUnique();
    }
}
