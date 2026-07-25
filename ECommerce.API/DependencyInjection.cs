using ECommerce.API.Middlewares;

namespace ECommerce.API;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddControllers();

        services.AddProblemDetails();
        services.AddExceptionHandler<GlobalExceptionMiddleware>();

        services.AddSwaggerGen();

        return services;
    }
}