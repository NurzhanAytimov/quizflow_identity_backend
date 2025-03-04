using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuizIdentity.Infrastructure.Persistence.Context;

namespace QuizIdentity.Infrastructure.Persistence.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefConnection");

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        services.InitRepasitories();
    }

    private static void InitRepasitories(this IServiceCollection services)
    {
        
    }
}
