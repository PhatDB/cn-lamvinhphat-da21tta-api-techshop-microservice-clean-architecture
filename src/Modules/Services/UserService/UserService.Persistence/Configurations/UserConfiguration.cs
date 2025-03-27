using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Entities;
using UserService.Domain.ValueObjects;

namespace UserService.Persistence.Configurations
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id).HasColumnName("user_id").IsRequired();

            builder.Property(u => u.Username).HasColumnName("username").IsRequired().HasMaxLength(255);

            builder.Property(u => u.Email).HasColumnName("email").HasConversion(email => email.Value, value => Email.Create(value).Value).IsRequired().HasMaxLength(255);

            builder.Property(u => u.Password).HasColumnName("password").HasConversion(password => password.Value, value => Password.Create(value).Value).IsRequired()
                .HasMaxLength(255);

            builder.Property(u => u.Role).HasColumnName("role").IsRequired().HasMaxLength(50);

            builder.Property(u => u.CreatedAt).HasColumnName("created_at").IsRequired();

            builder.Property(u => u.LastLogin).HasColumnName("last_login").IsRequired(false);

            builder.Property(u => u.IsActive).HasColumnName("is_active").IsRequired();

            builder.HasMany(u => u.UserAddresses).WithOne().HasForeignKey(ua => ua.UserId).OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("User");
        }
    }
}