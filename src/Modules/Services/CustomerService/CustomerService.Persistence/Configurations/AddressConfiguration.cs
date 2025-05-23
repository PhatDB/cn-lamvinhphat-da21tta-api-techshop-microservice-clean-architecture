using CustomerService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerService.Persistence.Configurations
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.ToTable("address");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Id).HasColumnName("address_id").IsRequired();

            builder.Property(a => a.CustomerId).HasColumnName("customer_id").IsRequired();

            builder.Property(a => a.Street).HasColumnName("street").HasMaxLength(255);

            builder.Property(a => a.Hamlet).HasColumnName("hamlet").HasMaxLength(100);

            builder.Property(a => a.City).HasColumnName("city").HasMaxLength(100);

            builder.Property(a => a.District).HasColumnName("district").HasMaxLength(100);

            builder.Property(a => a.Ward).HasColumnName("ward").HasMaxLength(100);

            builder.Property(a => a.CreatedAt).HasColumnName("created_at").IsRequired();

            builder.HasOne<Customer>().WithMany().HasForeignKey(ua => ua.CustomerId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}