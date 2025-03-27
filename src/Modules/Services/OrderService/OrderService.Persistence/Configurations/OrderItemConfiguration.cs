using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Domain.Entities;

namespace OrderService.Persistence.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("OrderItem");
            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id).HasColumnName("order_item_id").IsRequired();
            builder.Property(o => o.OrderId).HasColumnName("order_id").IsRequired();
            builder.Property(o => o.ProductId).HasColumnName("product_id").IsRequired();
            builder.Property(o => o.Quantity).HasColumnName("quantity").IsRequired();
            builder.Property(o => o.UnitPrice).HasColumnName("unit_price").HasColumnType("decimal(10,2)").IsRequired();
            builder.Property(o => o.TotalPrice).HasColumnName("total_price").IsRequired();
        }
    }
}