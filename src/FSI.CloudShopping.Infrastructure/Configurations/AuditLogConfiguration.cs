namespace FSI.CloudShopping.Infrastructure.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FSI.CloudShopping.Domain.Entities;

public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.HasKey(al => al.Id);

        builder.Property(al => al.EntityName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(al => al.EntityId)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(al => al.Action)
            .IsRequired();

        builder.Property(al => al.OldValues)
            .HasColumnType("TEXT");

        builder.Property(al => al.NewValues)
            .HasColumnType("TEXT");

        builder.Property(al => al.UserEmail)
            .HasMaxLength(254);

        builder.Property(al => al.IpAddress)
            .HasMaxLength(45);

        builder.Property(al => al.UserAgent)
            .HasMaxLength(500);

        builder.Property(al => al.CreatedAt)
            .HasDefaultValue(DateTime.UtcNow);

        // Indexes
        builder.HasIndex(al => al.EntityName);
        builder.HasIndex(al => al.EntityId);
        builder.HasIndex(al => al.UserId);
        builder.HasIndex(al => al.CreatedAt);
    }
}
