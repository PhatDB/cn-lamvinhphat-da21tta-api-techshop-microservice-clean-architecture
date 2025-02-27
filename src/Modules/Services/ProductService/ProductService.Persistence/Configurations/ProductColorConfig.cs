using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductService.Domain.Entities;

namespace ProductService.Persistence.Configurations
{
    public class ProductColorConfig : IEntityTypeConfiguration<ProductColor>
    {
        public void Configure(EntityTypeBuilder<ProductColor> builder)
        {
            builder.ToTable("ProductColor");
            builder.HasKey(pc => pc.Id);
            builder.Property(pc => pc.Id).HasColumnName("product_color_id");
            builder.Property(pc => pc.ProductId).HasColumnName("product_id").IsRequired();
            builder.Property(pc => pc.ColorId).HasColumnName("color_id").IsRequired();

            builder.HasOne<Product>().WithMany().HasForeignKey(pc => pc.ProductId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne<Color>().WithMany().HasForeignKey(pc => pc.ColorId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}