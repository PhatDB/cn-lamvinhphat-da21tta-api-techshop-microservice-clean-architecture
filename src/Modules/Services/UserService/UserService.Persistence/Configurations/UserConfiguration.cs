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
            builder.ToTable("User");

            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).HasColumnName("user_id");

            builder.HasIndex(u => u.Email).IsUnique();
            builder.HasIndex(u => u.Username).IsUnique();

            builder.Property(u => u.Username).HasColumnName("username").HasMaxLength(255).IsRequired();

            builder.Property(u => u.Email).HasColumnName("email").HasMaxLength(255).IsRequired().HasConversion(email => email.Value, value => Email.Create(value).Value);

            builder.Property(u => u.Password).HasConversion(password => password.Value, value => Password.Create(value).Value).HasColumnName("password").HasMaxLength(255)
                .IsRequired();

            builder.Property(u => u.Role).HasColumnName("role").HasMaxLength(50).HasDefaultValue("user").IsRequired();

            builder.Property(u => u.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("GETDATE()").IsRequired();

            builder.Property(u => u.LastLogin).HasColumnName("last_login").IsRequired(false);

            builder.Property(u => u.IsActive).HasColumnName("is_active").HasDefaultValue(true).IsRequired();

            builder.HasMany(u => u.UserAddresses).WithOne(a => a.User).HasForeignKey(a => a.UserId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}