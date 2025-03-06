using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductService.Domain.Entities;
using ProductService.Domain.ValueObjects;

namespace ProductService.Persistence.Configurations
{
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Product");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).HasColumnName("product_id");
            builder.Property(p => p.Name).HasColumnName("name").HasMaxLength(255)
                .IsRequired();
            builder.Property(p => p.Description).HasColumnName("description")
                .HasColumnType("TEXT");
            builder.Property(p => p.Price).HasColumnName("price")
                .HasColumnType("decimal(10,2)").IsRequired();
            builder.Property(p => p.DiscountPrice).HasColumnName("discount_price")
                .HasColumnType("decimal(10,2)");
            builder.Property(p => p.Sku)
                .HasConversion(sku => sku.Value, value => SKU.Create(value).Value)
                .HasColumnName("sku").HasMaxLength(100).IsRequired();
            builder.Property(p => p.SoldQuantity).HasColumnName("sold_quantity")
                .HasDefaultValue(0);
            builder.Property(p => p.CreatedAt).HasColumnName("created_at")
                .HasDefaultValueSql("GETDATE()").IsRequired();
            builder.Property(p => p.IsActive).HasColumnName("is_active")
                .HasDefaultValue(true).IsRequired();
            builder.Property(p => p.CategoryId).HasColumnName("category_id").IsRequired();

            builder.HasOne<Category>().WithMany().HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.ProductImages).WithOne()
                .HasForeignKey(pi => pi.ProductId).OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.Inventory).WithOne(i => i.Product)
                .HasForeignKey<Inventory>(i => i.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}