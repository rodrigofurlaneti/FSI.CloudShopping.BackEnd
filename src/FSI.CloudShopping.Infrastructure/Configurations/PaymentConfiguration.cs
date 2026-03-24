namespace FSI.CloudShopping.Infrastructure.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FSI.CloudShopping.Domain.Entities;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.OrderId)
            .IsRequired();

        builder.Property(p => p.Method)
            .IsRequired();

        builder.OwnsOne(p => p.Amount, a =>
        {
            a.Property(x => x.Amount)
                .HasColumnName("Amount")
                .HasPrecision(18, 2);

            a.Property(x => x.Currency)
                .HasColumnName("AmountCurrency")
                .HasMaxLength(3)
                .HasDefaultValue("BRL");
        });

        builder.Property(p => p.Status)
            .IsRequired();

        builder.Property(p => p.TransactionId)
            .HasMaxLength(100);

        builder.Property(p => p.GatewayResponse)
            .HasMaxLength(1000);

        builder.Property(p => p.FailureReason)
            .HasMaxLength(500);

        builder.Property(p => p.RetryCount)
            .HasDefaultValue(0);

        // Indexes
        builder.HasIndex(p => p.OrderId);
        builder.HasIndex(p => p.TransactionId);
        builder.HasIndex(p => p.Status);
    }
}
