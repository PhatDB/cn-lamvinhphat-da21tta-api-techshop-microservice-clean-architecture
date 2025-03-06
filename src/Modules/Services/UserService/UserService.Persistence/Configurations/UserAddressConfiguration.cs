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
            builder.ToTable("UserAddress");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id).HasColumnName("useraddress_id");

            builder.Property(a => a.AddressLine).HasColumnName("addressline")
                .HasMaxLength(255).IsRequired();

            builder.Property(a => a.PhoneNumber)
                .HasConversion(phone => phone.Value,
                    value => PhoneNumber.Create(value).Value)
                .HasColumnName("phone_number").HasMaxLength(15).IsRequired(false);

            builder.Property(a => a.Province).HasColumnName("province").HasMaxLength(50)
                .IsRequired();

            builder.Property(a => a.District).HasColumnName("district").HasMaxLength(50)
                .IsRequired();

            builder.Property(a => a.UserId).HasColumnName("user_id").IsRequired();

            builder.HasOne(a => a.User).WithMany(u => u.UserAddresses)
                .HasForeignKey(a => a.UserId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}