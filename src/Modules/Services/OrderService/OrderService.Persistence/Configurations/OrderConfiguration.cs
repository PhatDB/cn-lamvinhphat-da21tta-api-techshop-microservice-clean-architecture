using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Domain.Entities;

namespace OrderService.Persistence.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Order");

            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id).HasColumnName("order_id");
            builder.Property(o => o.TotalAmount).HasColumnName("total_amount").HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(o => o.OrderStatus).HasColumnName("order_status").HasConversion<int>();
            builder.Property(o => o.UserId).HasColumnName("user_id");
            builder.Property(o => o.CreatedAt).HasColumnName("created_at").HasColumnType("datetime").IsRequired();
            builder.Property(o => o.Street).HasColumnName("street").IsRequired();
            builder.Property(o => o.City).HasColumnName("city").IsRequired();
            builder.Property(o => o.District).HasColumnName("district").IsRequired();
            builder.Property(o => o.Ward).HasColumnName("ward").IsRequired();
            builder.Property(o => o.ZipCode).HasColumnName("zip_code").IsRequired(false);
            builder.Property(o => o.PhoneNumber).HasColumnName("phone_number").IsRequired();
            builder.Property(o => o.BuyerName).HasColumnName("buyer_name").IsRequired();
        }
    }
}