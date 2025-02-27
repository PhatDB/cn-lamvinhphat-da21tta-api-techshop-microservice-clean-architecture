using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductService.Domain.Entities;

namespace ProductService.Persistence.Configurations
{
    public class InventoryConfig : IEntityTypeConfiguration<Inventory>
    {
        public void Configure(EntityTypeBuilder<Inventory> builder)
        {
            builder.ToTable("Inventory");
            builder.HasKey(i => i.Id);
            builder.Property(i => i.Id).HasColumnName("inventory_id");
            builder.Property(i => i.ProductId).HasColumnName("product_id").IsRequired();
            builder.Property(i => i.StockQuantity).HasColumnName("stock_quantity").IsRequired();
            builder.Property(i => i.LastUpdated).HasColumnName("last_updated").HasDefaultValueSql("GETDATE()").IsRequired();

            builder.HasOne<Product>().WithMany().HasForeignKey(i => i.ProductId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}