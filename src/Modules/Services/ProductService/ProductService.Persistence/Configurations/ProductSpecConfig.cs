using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductService.Domain.Entities;

namespace ProductService.Persistence.Configurations
{
    public class ProductSpecConfig : IEntityTypeConfiguration<ProductSpec>
    {
        public void Configure(EntityTypeBuilder<ProductSpec> builder)
        {
            builder.ToTable("product_spec");
            builder.HasKey(ps => ps.Id);

            builder.Property(ps => ps.Id).HasColumnName("spec_id");

            builder.Property(ps => ps.ProductId).HasColumnName("product_id").IsRequired();

            builder.Property(ps => ps.SpecName).HasColumnName("spec_name").HasMaxLength(255).IsRequired();

            builder.Property(ps => ps.SpecValue).HasColumnName("spec_value").HasMaxLength(255);

            builder.HasOne<Product>().WithMany(p => p.ProductSpecs).HasForeignKey(ps => ps.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}