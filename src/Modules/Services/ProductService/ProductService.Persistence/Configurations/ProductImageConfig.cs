using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductService.Domain.Entities;

namespace ProductService.Persistence.Configurations
{
    public class ProductImageConfig : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            builder.ToTable("product_image");
            builder.HasKey(pi => pi.Id);

            builder.Property(pi => pi.Id).HasColumnName("image_id");

            builder.Property(pi => pi.ImageUrl).HasColumnName("image_url").HasMaxLength(300).IsRequired();

            builder.Property(pi => pi.ProductId).HasColumnName("product_id").IsRequired();

            builder.Property(pi => pi.IsMain).HasColumnName("is_main");

            builder.Property(pi => pi.SortOrder).HasColumnName("sort_order");

            builder.HasOne<Product>().WithMany(p => p.ProductImages).HasForeignKey(pi => pi.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}