using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizIdentity.Domain.Entities.Identity;

namespace QuizIdentity.Infrastructure.Persistence.Configurations.Identity;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.Property(u => u.UserName).HasColumnName("UserName").HasMaxLength(50);
        builder.Property(u => u.Email).HasColumnName("Email").HasMaxLength(50);
        builder.Property(u => u.Password).HasColumnName("Password").IsRequired();

        builder.HasMany(e => e.UserRoles)
            .WithOne(e => e.User)
            .HasForeignKey(ur => ur.UserId)
            .IsRequired();
    }
}
