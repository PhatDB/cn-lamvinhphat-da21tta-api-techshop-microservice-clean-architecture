using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Entities;
using UserService.Domain.ValueObjects;

namespace UserService.Persistence.Configurations
{
    public class UserAddressConfig : IEntityTypeConfiguration<UserAddress>
    {
        public void Configure(EntityTypeBuilder<UserAddress> builder)
        {
            builder.HasKey(ua => ua.Id);
            builder.Property(ua => ua.Id).HasColumnName("useraddress_id").IsRequired();

            builder.Property(ua => ua.UserId).HasColumnName("user_id").IsRequired();

            builder.Property(ua => ua.Street).HasColumnName("street").IsRequired().HasMaxLength(255);

            builder.Property(ua => ua.City).HasColumnName("city").IsRequired().HasMaxLength(100);

            builder.Property(ua => ua.District).HasColumnName("district").IsRequired().HasMaxLength(100);

            builder.Property(ua => ua.Ward).HasColumnName("ward").IsRequired().HasMaxLength(100);

            builder.Property(ua => ua.ZipCode).HasColumnName("zip_code").IsRequired().HasMaxLength(20);

            builder.Property(ua => ua.PhoneNumber).HasColumnName("phone_number").HasConversion(phone => phone.Value, value => PhoneNumber.Create(value).Value).IsRequired()
                .HasMaxLength(20);

            builder.HasOne<User>().WithMany().HasForeignKey(ua => ua.UserId).OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("UserAddress");
        }
    }
}