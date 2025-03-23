using System.Reflection;
using Microsoft.EntityFrameworkCore;
using QuizIdentity.Domain.Entities.Domain;
using QuizIdentity.Domain.Entities.Identity;
using QuizIdentity.Domain.Entities.Subscriptions;

namespace QuizIdentity.Infrastructure.Persistence.Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    #region Identity

    public DbSet<User> Users { get; set; }

    public DbSet<Role> Roles { get; set; }

    public DbSet<UserRole> UserRoles { get; set; }

    public DbSet<UserToken> UserTokens { get; set; }

    #endregion

    #region Subscriptions

    public DbSet<Subscription> Subscriptions { get; set; }

    #endregion

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    if (!optionsBuilder.IsConfigured)
    //    {
    //        optionsBuilder.UseNpgsql(
    //            @"Server=10.9.42.119:5432;Database=Kegoc-Mock;User ID=developer;Password=C5WkJ8sz"
    //        );
    //    }

    //    base.OnConfiguring(optionsBuilder);
    //}

    public override int SaveChanges()
    {
        OnBeforeSaving();

        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        OnBeforeSaving();

        return base.SaveChangesAsync(cancellationToken);
    }

    private void OnBeforeSaving()
    {
        ChangeTracker.DetectChanges();

        var entries = ChangeTracker.Entries().ToArray();

        foreach (var entry in entries)
        {
            var entity = entry.Entity;

            switch (entry.State)
            {
                case EntityState.Deleted when entity is ISoftDelete track:
                    {
                        track.IsDeleted = true;
                        track.DeleteDateUtc = DateTime.UtcNow;

                        entry.State = EntityState.Modified;
                        break;
                    }
                case EntityState.Modified when entity is IUpdated track:
                    {
                        track.UpdateDateUtc = DateTime.UtcNow;
                        break;
                    }
                case EntityState.Added when entity is ICreated track:
                    {
                        track.CreateDateUtc = DateTime.UtcNow;
                        break;
                    }
            }
        }
    }
}
