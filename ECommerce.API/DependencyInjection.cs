using ECommerce.API.Middlewares;
using ECommerce.Infrastructure.Persistence.Seeding;

namespace ECommerce.API;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddControllers();

        services.AddProblemDetails();
        services.AddExceptionHandler<GlobalExceptionMiddleware>();

        return services;
    }
}