using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductService.Domain.Entities;

namespace ProductService.Persistence.Configurations
{
    public class BrandConfig : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            builder.ToTable("brand");

            builder.HasKey(b => b.Id);

            builder.Property(b => b.Id).HasColumnName("brand_id");

            builder.Property(b => b.BrandName).HasColumnName("brand_name").HasMaxLength(100).IsRequired();

            builder.Property(b => b.Description).HasColumnName("description").HasColumnType("NVARCHAR(MAX)");

            builder.Property(b => b.IsActive).HasColumnName("is_active").HasDefaultValue(true).IsRequired();
        }
    }
}