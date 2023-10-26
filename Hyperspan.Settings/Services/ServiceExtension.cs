using Hyperspan.Settings.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Hyperspan.Settings.Services
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddSettingsServices(this IServiceCollection services)
        {
            services.AddTransient<ISettingService, SettingsService>();
            return services;
        }
    }
}
