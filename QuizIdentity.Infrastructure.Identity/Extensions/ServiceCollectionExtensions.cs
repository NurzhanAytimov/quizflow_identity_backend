using Microsoft.Extensions.DependencyInjection;
using QuizIdentity.Infrastructure.Identity.Services;
using QuizIdentity.Infrastructure.Identity.Services.Interfaces;

namespace QuizIdentity.Infrastructure.Identity.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddIdentityInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddHttpContextAccessor();
    }
}
