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

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("cart_id").IsRequired();

            builder.Property(x => x.CustomerId).HasColumnName("customer_id").IsRequired();

            builder.Property(p => p.CreatedAt).HasColumnName("created_at").IsRequired();
        }
    }
}