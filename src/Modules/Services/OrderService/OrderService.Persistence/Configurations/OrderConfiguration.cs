using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Domain.Entities;

namespace OrderService.Persistence.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("order");

            builder.HasKey(o => o.Id);

            builder.Property(o => o.Id).HasColumnName("order_id");

            builder.Property(o => o.CustomerId).HasColumnName("customer_id").IsRequired();

            builder.Property(o => o.Status).HasColumnName("status").HasConversion<byte>().IsRequired();

            builder.Property(o => o.TotalAmount).HasColumnName("total_amount").HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(o => o.CreatedAt).HasColumnName("created_at").HasColumnType("datetime").IsRequired();
        }
    }
}