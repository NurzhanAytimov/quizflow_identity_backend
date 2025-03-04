using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizIdentity.Domain.Entities.Identity;

namespace QuizIdentity.Infrastructure.Persistence.Configurations.Identity;

internal sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasColumnName("Name");

        builder.HasMany(e => e.UserRoles)
                   .WithOne(e => e.Role)
                   .HasForeignKey(ur => ur.RoleId)
                   .IsRequired();
    }
}
