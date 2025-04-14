using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductService.Domain.Entities;

namespace ProductService.Persistence.Configurations
{
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("product");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).HasColumnName("product_id");

            builder.Property(p => p.ProductName).HasColumnName("product_name").HasMaxLength(200).IsRequired();

            builder.Property(p => p.Description).HasColumnName("description").HasColumnType("NVARCHAR(MAX)");

            builder.Property(p => p.Specs).HasColumnName("specs").HasColumnType("NVARCHAR(MAX)");

            builder.Property(p => p.Price).HasColumnName("price").HasColumnType("decimal(18,2)").IsRequired();

            builder.Property(p => p.Discount).HasColumnName("discount").HasColumnType("decimal(5,2)");

            builder.Property(p => p.SoldQuantity).HasColumnName("sold_quantity").HasDefaultValue(0);

            builder.Property(p => p.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("GETDATE()").IsRequired();

            builder.Property(p => p.IsActive).HasColumnName("is_active").HasDefaultValue(true).IsRequired();

            builder.Property(p => p.IsFeatured).HasColumnName("is_featured").HasDefaultValue(false).IsRequired();

            builder.Property(p => p.CategoryId).HasColumnName("category_id").IsRequired();

            builder.Property(p => p.BrandId).HasColumnName("brand_id").IsRequired();

            builder.HasOne<Category>().WithMany().HasForeignKey(p => p.CategoryId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne<Brand>().WithMany().HasForeignKey(p => p.BrandId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}