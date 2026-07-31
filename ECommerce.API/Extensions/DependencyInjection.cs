using ECommerce.API.Middlewares;

namespace ECommerce.API.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddControllers();

        services.AddProblemDetails();
        services.AddExceptionHandler<GlobalExceptionMiddleware>();

        services.AddSwaggerGen();

        services.AddApiVersioningConfig();

        return services;
    }
}