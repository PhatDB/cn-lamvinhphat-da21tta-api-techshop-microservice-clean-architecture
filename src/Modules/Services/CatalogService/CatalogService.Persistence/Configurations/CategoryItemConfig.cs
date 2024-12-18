using CatalogService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CatalogService.Persistence.Configurations
{
    public class CategoryItemConfig : IEntityTypeConfiguration<CategoryItem>
    {
        public void Configure(EntityTypeBuilder<CategoryItem> builder)
        {
            builder.ToTable("CategoryItem");
            builder.Ignore(ci => ci.DomainEvents);
            builder.HasKey(ci => new { ci.ProductId, ci.CategoryId });
            builder.Property(ci => ci.CategoryId).HasColumnName("CategoryID").IsRequired();
            builder.Property(ci => ci.ProductId).HasColumnName("ProductID").IsRequired();
            builder.Property(ci => ci.CreatedDate).HasColumnName("CreatedDate").IsRequired();
            
            builder.HasOne<Category>()
                .WithMany()
                .HasForeignKey(ci => ci.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<Product>()
                .WithMany()
                .HasForeignKey(ci => ci.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}