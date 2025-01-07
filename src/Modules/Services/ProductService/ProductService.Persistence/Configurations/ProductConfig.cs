using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductService.Domain.Entities;

namespace ProductService.Persistence.Configurations
{
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Product");
            builder.Ignore(p => p.DomainEvents);
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).HasColumnName("ProductID");
            builder.Property(p => p.ProductName).HasColumnName("ProductName").HasMaxLength(255).IsRequired();
            builder.Property(p => p.Description).HasColumnName("Description").HasMaxLength(500);
            builder.Property(p => p.Price).HasColumnName("Price").IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(p => p.DiscountPrice).HasColumnName("DiscountPrice").HasColumnType("decimal(18,2)");
            builder.Property(p => p.SKU).HasColumnName("SKU").HasMaxLength(50);
            builder.Property(p => p.Brand).HasColumnName("Brand").HasMaxLength(100);
            builder.Property(p => p.Model).HasColumnName("Model").HasMaxLength(100);
            builder.Property(p => p.StockStatus).HasColumnName("StockStatus");
            builder.Property(p => p.CreatedDate).HasColumnName("CreatedDate").IsRequired();
            builder.Property(p => p.UpdatedDate).HasColumnName("UpdatedDate");
            builder.Property(p => p.IsActive).HasColumnName("IsActive").IsRequired().HasDefaultValue(1);

            builder.HasMany(p => p.ProductImages).WithOne().HasForeignKey(pi => pi.ProductId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}