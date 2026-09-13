using ECommerce.API.Filters;
using ECommerce.API.Middlewares;
using ECommerce.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using System.Reflection;
using System.Text.Json.Serialization;

namespace ECommerce.API.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration config)
    {
        services.AddControllers(options =>
        {
            options.Filters.Add<AuditActionFilter>();
        })
        .AddJsonOptions(options =>
        {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        services.AddProblemDetails();
        services.AddExceptionHandler<GlobalExceptionMiddleware>();

        services.AddSwaggerGen(options =>
        {
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);
        });

        services.AddApiVersioningConfig();

        services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
        {
            options.Password.RequiredLength = 8;
            options.User.RequireUniqueEmail = true;
            options.SignIn.RequireConfirmedEmail = true;
        })
            .AddEntityFrameworkStores<IdentityStoreDbContext>()
            .AddDefaultTokenProviders();

        services.AddAuthenticationAndAuthorization(config);
        return services;
    }
}