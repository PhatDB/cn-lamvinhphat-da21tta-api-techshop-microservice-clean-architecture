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

            builder.Property(o => o.CustomerId).HasColumnName("customer_id");

            builder.Property(o => o.Status).HasColumnName("status").HasConversion<byte>().IsRequired();

            builder.Property(o => o.TotalAmount).HasColumnName("total_amount").HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(o => o.CreatedAt).HasColumnName("created_at").HasColumnType("datetime").IsRequired();

            builder.Property(o => o.ReceiverName).HasColumnName("receiver_name").HasMaxLength(255).IsRequired();

            builder.Property(o => o.ReceiverPhone).HasColumnName("receiver_phone").HasMaxLength(20).IsRequired(false);

            builder.Property(o => o.ReceiverAddress).HasColumnName("receiver_address").HasMaxLength(500)
                .IsRequired(false);

            builder.Property(o => o.Note).HasColumnName("note").HasMaxLength(1000).IsRequired(false);

            builder.Property(o => o.SessionId).HasColumnName("session_id").HasMaxLength(255).IsRequired(false);

            builder.Property(o => o.PaymentMethod).HasColumnName("payment_method").HasConversion<byte>().IsRequired();
        }
    }
}