using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductService.Domain.Entities;

namespace ProductService.Persistence.Configurations
{
    public class CategoryConfig : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("category");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("category_id");

            builder.Property(c => c.CategoryName).HasColumnName("category_name").HasMaxLength(100).IsRequired();

            builder.Property(c => c.Description).HasColumnName("description").HasColumnType("NVARCHAR(MAX)");

            builder.Property(c => c.ImageUrl).HasColumnName("image_url").HasMaxLength(300);

            builder.Property(c => c.IsActive).HasColumnName("is_active").HasDefaultValue(true).IsRequired();

            builder.Property(c => c.ParentId).HasColumnName("parent_id");

            builder.HasMany(c => c.Subcategories).WithOne().HasForeignKey(c => c.ParentId);
        }
    }
}