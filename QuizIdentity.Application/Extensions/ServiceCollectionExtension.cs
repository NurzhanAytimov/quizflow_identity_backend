using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using QuizIdentity.Application.Services;
using QuizIdentity.Application.Services.Interfaces;
using MediatR;
using QuizIdentity.Application.Behaviours;
using QuizIdentity.Application.Features.Account.Commands.Validation;
using FluentValidation.AspNetCore;

namespace QuizIdentity.Application.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddApplicationLayer(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services.AddScoped<IPasswordEncryptor, PasswordEncryptor>();

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        //services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddValidatorsFromAssemblyContaining<CreateAccountValidation>();

        services.AddFluentValidationAutoValidation(config =>
        {
            config.DisableDataAnnotationsValidation = true;
        });

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly(), includeInternalTypes: true);

    }
}
