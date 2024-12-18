using CatalogService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CatalogService.Persistence.Configurations
{
    public class CategoryConfig : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Category");
            builder.Ignore(c => c.DomainEvents);
            builder.HasKey(c=>c.Id);
            builder.Property(c => c.Id).HasColumnName("CategoryID");
            builder.Property(c => c.CategoryName).HasColumnName("CategoryName").HasMaxLength(100).IsRequired();
            builder.Property(c => c.Description).HasColumnName("Description").HasMaxLength(255);
            builder.Property(c => c.CreatedDate).HasColumnName("CreatedDate").IsRequired();
            builder.Property(c => c.UpdatedDate).HasColumnName("UpdatedDate");
            builder.Property(c => c.IsActive).HasColumnName("IsActive").IsRequired().HasDefaultValue(1);
            builder.Property(c=>c.ParentCategoryId).HasColumnName("ParentCategoryID");
        }
    }
}