using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Domain.Entities;

namespace OrderService.Persistence.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("order_item");

            builder.HasKey(oi => oi.Id);
            builder.Property(oi => oi.Id).HasColumnName("order_item_id").IsRequired();
            builder.Property(oi => oi.OrderId).HasColumnName("order_id").IsRequired();
            builder.Property(oi => oi.ProductId).HasColumnName("product_id").IsRequired();
            builder.Property(oi => oi.Quantity).HasColumnName("quantity").IsRequired();
            builder.Property(oi => oi.Price).HasColumnName("price").HasColumnType("decimal(18,2)").IsRequired();

            builder.HasOne<Order>().WithMany(o => o.OrderItems).HasForeignKey(pi => pi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}