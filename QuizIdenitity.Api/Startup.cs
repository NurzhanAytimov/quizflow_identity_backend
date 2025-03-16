using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using QuizIdentity.Domain.Settings;

namespace QuizIdenitity.Api;

public static class Startup
{
    public static void AddAuthenticationAndAuthorization(this IServiceCollection services, WebApplicationBuilder builder)
    {
        var options = builder.Configuration.GetSection(JWTSettings.DefaultSection).Get<JWTSettings>();
        var jwtKey = options.Secret;
        var issuer = options.Issuer;
        var audience = options.Audience;

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(o =>
        {
            var options = builder.Configuration.GetSection(JWTSettings.DefaultSection).Get<JWTSettings>();
            var jwtKey = options.Secret;
            var issuer = options.Issuer;
            var audience = options.Audience;
            o.RequireHttpsMetadata = true;
            o.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true
            };
            //o.Events = new JwtBearerEvents
            //{
            //    OnMessageReceived = context =>
            //    {
            //        var accessToken = context.Request.Query["access_token"];
            //        var path = context.HttpContext.Request.Path;
            //        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/notificationHub"))
            //        {
            //            context.Token = accessToken;
            //        }
            //        return Task.CompletedTask;
            //    }
            //};
        });

        services.AddAuthentication();
    }

    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
            c.SwaggerDoc(
                "v1",
                new OpenApiInfo()
                {
                    Version = "v1",
                    Title = "QuizFlow.Api",
                    Description = "This is version 1.0",
                    TermsOfService = new Uri("http://localhost:5004/"),
                    Contact = new OpenApiContact()
                    {
                        Name = "Test contact",
                        Url = new Uri("http://localhost:5004/"),
                    },
                }
            );
            c.AddSecurityDefinition(
                "Bearer",
                new OpenApiSecurityScheme()
                {
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' followed by a space and then your JWT token.",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer",
                }
            );
            c.AddSecurityRequirement(
                new OpenApiSecurityRequirement
                {
            {
                new OpenApiSecurityScheme()
                {
                    Reference = new OpenApiReference()
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer",
                    },
                    Name = "Bearer",
                    In = ParameterLocation.Header,
                },
                Array.Empty<string>()
            },
                }
            );
        });
    }
}
