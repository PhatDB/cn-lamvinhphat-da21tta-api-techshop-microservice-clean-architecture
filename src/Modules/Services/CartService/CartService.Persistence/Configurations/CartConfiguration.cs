using CartService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CartService.Persistence.Configurations
{
    public class CartConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.ToTable("cart");

            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).HasColumnName("cart_id").IsRequired();

            builder.Property(c => c.CustomerId).HasColumnName("customer_id");

            builder.Property(c => c.CreatedAt).HasColumnName("created_at");

            builder.Property(c => c.SessionId).HasColumnName("session_id");
        }
    }
}