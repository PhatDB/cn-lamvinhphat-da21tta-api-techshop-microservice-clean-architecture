using CustomerService.Domain.Entities;
using CustomerService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerService.Persistence.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("customer");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("customer_id").IsRequired();

            builder.Property(c => c.CustomerName).HasColumnName("customer_name").HasMaxLength(100);
            ;

            builder.Property(c => c.Email).HasColumnName("email")
                .HasConversion(email => email.Value, value => Email.Create(value).Value).HasMaxLength(100);

            builder.Property(c => c.Password).HasColumnName("password")
                .HasConversion(password => password.Value, value => Password.Create(value).Value).HasMaxLength(255);

            builder.Property(c => c.Phone).HasColumnName("phone")
                .HasConversion(phone => phone.Value, value => PhoneNumber.Create(value).Value).HasMaxLength(20);

            builder.Property(u => u.Status).HasColumnName("status");

            builder.Property(u => u.Otp).HasColumnName("otp");

            builder.Property(u => u.OtpExpired).HasColumnName("otp_expired");
        }
    }
}