using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductService.Domain.Entities;

namespace ProductService.Persistence.Configurations
{
    public class ProductImageConfig : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            builder.ToTable("ProductImage");
            builder.Ignore(pi => pi.DomainEvents);
            builder.HasKey(pi => pi.Id);
            builder.Property(pi => pi.Id).HasColumnName("ImageID");
            builder.Property(pi => pi.ProductId).HasColumnName("ProductID").IsRequired();
            builder.Property(pi => pi.ImageUrl).HasColumnName("ImageUrl").HasMaxLength(255).IsRequired();
            builder.Property(pi => pi.Position).HasColumnName("Position");
            builder.Property(pi => pi.CreatedDate).HasColumnName("CreatedDate").IsRequired();
            builder.Property(pi => pi.UpdatedDate).HasColumnName("UpdatedDate");

            builder.HasOne<Product>().WithMany(p => p.ProductImages).HasForeignKey(pi => pi.ProductId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}