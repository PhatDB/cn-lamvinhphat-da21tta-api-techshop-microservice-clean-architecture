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

            builder.HasKey(oi => oi.Id);
            builder.Property(oi => oi.Id).HasColumnName("order_item_id");
            builder.Property(oi => oi.OrderId).HasColumnName("order_id").IsRequired();
            builder.Property(oi => oi.ProductId).HasColumnName("product_id").IsRequired();
            builder.Property(oi => oi.ProductName).HasColumnName("product_name").IsRequired();
            builder.Property(oi => oi.Quantity).HasColumnName("quantity").IsRequired();
            builder.Property(oi => oi.UnitPrice).HasColumnName("unit_price").HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(oi => oi.TotalPrice).HasColumnName("total_price").IsRequired();
            builder.HasOne<Order>().WithMany(o => o.OrderItems).HasForeignKey(oi => oi.OrderId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}