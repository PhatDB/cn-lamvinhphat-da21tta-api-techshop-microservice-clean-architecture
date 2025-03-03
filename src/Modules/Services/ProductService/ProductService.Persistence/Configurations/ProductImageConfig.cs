using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductService.Domain.Entities;

namespace ProductService.Persistence.Configurations
{
    public class ProductImageConfig : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            builder.ToTable("ProductImage");
            builder.HasKey(pi => pi.Id);
            builder.Property(pi => pi.Id).HasColumnName("product_image_id");
            builder.Property(pi => pi.Title).HasColumnName("title").HasMaxLength(255);
            builder.Property(pi => pi.Position).HasColumnName("position");
            builder.Property(pi => pi.ImageUrl).HasColumnName("image_url").IsRequired();
            builder.Property(pi => pi.ProductId).HasColumnName("product_id").IsRequired();

            builder.HasOne<Product>().WithMany(p => p.ProductImages)
                .HasForeignKey(pi => pi.ProductId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}