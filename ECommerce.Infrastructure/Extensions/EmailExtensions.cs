using ECommerce.Infrastructure.Caching;
using ECommerce.Infrastructure.Services;
using ECommerce.UseCases.Common.Interfaces;
using ECommerce.UseCases.Common.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ECommerce.Infrastructure.Extensions;

public static class EmailExtensions
{
    public static IServiceCollection AddEmailConfigurations(this IServiceCollection services)
    {
        services.AddOptions<MailSettings>()
            .BindConfiguration(MailSettings.SectionName)
            .ValidateOnStart();
        services.AddSingleton<IValidateOptions<MailSettings>, MailSettingsValidator>();

        services.AddOptions<EmailVerificationSettings>()
            .BindConfiguration(EmailVerificationSettings.SectionName)
            .ValidateOnStart();
        services.AddSingleton<IValidateOptions<EmailVerificationSettings>, EmailVerificationSettingsValidator>();

        services.AddTransient<IEmailService, EmailService>();
        services.AddSingleton<IPendingRegistrationStore, HybridPendingRegistrationStore>();
        return services;

    }
}
