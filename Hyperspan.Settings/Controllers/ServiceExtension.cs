using Hyperspan.Settings.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Hyperspan.Settings.Controllers;

public static class ServiceExtension
{
    public static IServiceCollection AddSettingsApi(this IServiceCollection services)
    {
        services.AddSettingsServices();
        return services;
    }
}