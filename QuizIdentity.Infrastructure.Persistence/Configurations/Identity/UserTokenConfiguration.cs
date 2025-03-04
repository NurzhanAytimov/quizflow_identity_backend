using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizIdentity.Domain.Entities.Identity;

namespace QuizIdentity.Infrastructure.Persistence.Configurations.Identity;

internal sealed class UserTokenConfiguration : IEntityTypeConfiguration<UserToken>
{
    public void Configure(EntityTypeBuilder<UserToken> builder)
    {
        builder.ToTable("UserTokens");

        builder.Property(e => e.RefreshToken).HasColumnName("RefreshToken");

        builder
            .HasOne(e => e.User)
            .WithOne(e => e.UserToken)
            .HasForeignKey<UserToken>(e => e.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
