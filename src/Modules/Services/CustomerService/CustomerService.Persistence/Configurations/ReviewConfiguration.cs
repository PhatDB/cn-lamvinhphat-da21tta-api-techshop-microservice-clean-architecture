using CustomerService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerService.Persistence.Configurations
{
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.ToTable("reviews");
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Id).HasColumnName("review_id").IsRequired();

            builder.Property(r => r.CustomerId).HasColumnName("customer_id").IsRequired();

            builder.Property(r => r.ProductId).HasColumnName("product_id").IsRequired();

            builder.Property(r => r.Comment).HasColumnName("comment");

            builder.Property(r => r.IsVerified).HasColumnName("is_verified");

            builder.Property(r => r.CreatedAt).HasColumnName("created_at");

            builder.Property(r => r.Rating).HasColumnName("rating").HasColumnType("TINYINT");

            builder.HasOne<Customer>().WithMany(c => c.Reviews).HasForeignKey(ua => ua.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}