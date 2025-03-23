using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizIdentity.Domain.Entities.Subscriptions;

namespace QuizIdentity.Infrastructure.Persistence.Configurations.Subscriptions;

internal sealed class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
{
    public void Configure(EntityTypeBuilder<Subscription> builder)
    {
        builder.ToTable("Subscriptions");

        builder.HasOne(e => e.User)
            .WithOne(e => e.Subscription)
            .HasForeignKey<Subscription>(ur => ur.UserId)
            .IsRequired();
    }
}
