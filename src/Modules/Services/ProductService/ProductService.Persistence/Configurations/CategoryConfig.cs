using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductService.Domain.Entities;

namespace ProductService.Persistence.Configurations
{
    public class CategoryConfig : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Category");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).HasColumnName("category_id");
            builder.Property(c => c.Name).HasColumnName("name").HasMaxLength(255)
                .IsRequired();
            builder.Property(c => c.Description).HasColumnName("description")
                .HasColumnType("TEXT");
            builder.Property(c => c.CreatedAt).HasColumnName("created_at")
                .HasDefaultValueSql("GETDATE()").IsRequired();
            builder.Property(c => c.IsActive).HasColumnName("is_active")
                .HasDefaultValue(true).IsRequired();
        }
    }
}