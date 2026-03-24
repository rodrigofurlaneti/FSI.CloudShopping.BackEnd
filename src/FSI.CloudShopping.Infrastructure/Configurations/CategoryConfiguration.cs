namespace FSI.CloudShopping.Infrastructure.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FSI.CloudShopping.Domain.Entities;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.OwnsOne(c => c.Slug, s =>
        {
            s.Property(x => x.Value)
                .HasColumnName("Slug")
                .HasMaxLength(200)
                .IsRequired();
        });

        builder.Property(c => c.Description)
            .HasMaxLength(1000);

        builder.Property(c => c.ParentCategoryId);

        builder.Property(c => c.IsActive)
            .HasDefaultValue(true);

        builder.Property(c => c.SortOrder)
            .HasDefaultValue(0);

        builder.Property(c => c.ImageUrl)
            .HasMaxLength(500);

        // Relationships
        builder.HasOne(c => c.ParentCategory)
            .WithMany(c => c.ChildCategories)
            .HasForeignKey(c => c.ParentCategoryId)
            .OnDelete(DeleteBehavior.SetNull);

        // Indexes
        builder.HasIndex(c => c.Slug)
            .IsUnique();

        builder.HasIndex(c => c.ParentCategoryId);
        builder.HasIndex(c => c.IsActive);
    }
}
