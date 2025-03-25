using CartService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CartService.Persistence.Configurations
{
    public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.ToTable("CartItem");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("cart_item_id").IsRequired();
            builder.Property(x => x.CartId).HasColumnName("cart_id").IsRequired();
            builder.Property(x => x.ProductId).HasColumnName("product_id").IsRequired();
            builder.Property(x => x.Quantity).HasColumnName("quantity").IsRequired();
            builder.Property(x => x.UnitPrice).HasColumnName("unit_price").HasColumnType("decimal(10,2)").IsRequired();
        }
    }
}