using Microsoft.Extensions.DependencyInjection;
using QuizIdentity.Infrastructure.Shared.Services.Implementations;
using QuizIdentity.Infrastructure.Shared.Services.Interfaces;

namespace QuizIdentity.Infrastructure.Shared.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddSharedInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IEmailService, EmailService>();
    }
}
