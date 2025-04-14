using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductService.Domain.Entities;

namespace ProductService.Persistence.Configurations
{
    public class CategoryBrandConfig : IEntityTypeConfiguration<CategoryBrand>
    {
        public void Configure(EntityTypeBuilder<CategoryBrand> builder)
        {
            builder.ToTable("category_brand");

            builder.Ignore(cb => cb.Id);

            builder.HasKey(cb => new { cb.CategoryId, cb.BrandId });

            builder.Property(cb => cb.CategoryId).HasColumnName("category_id").IsRequired();

            builder.Property(cb => cb.BrandId).HasColumnName("brand_id").IsRequired();

            builder.HasOne<Category>().WithMany().HasForeignKey(cb => cb.CategoryId).OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<Brand>().WithMany().HasForeignKey(cb => cb.BrandId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}