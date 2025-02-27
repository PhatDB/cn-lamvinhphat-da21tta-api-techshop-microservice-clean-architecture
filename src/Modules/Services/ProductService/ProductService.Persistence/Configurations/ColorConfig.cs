using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductService.Domain.Entities;

namespace ProductService.Persistence.Configurations
{
    public class ColorConfig : IEntityTypeConfiguration<Color>
    {
        public void Configure(EntityTypeBuilder<Color> builder)
        {
            builder.ToTable("Color");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).HasColumnName("color_id");
            builder.Property(c => c.Name).HasColumnName("name").HasMaxLength(50).IsRequired();
        }
    }
}