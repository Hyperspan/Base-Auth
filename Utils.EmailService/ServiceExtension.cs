using Microsoft.Extensions.DependencyInjection;

namespace Utils.EmailService
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddEmailUtility(this IServiceCollection services)
        {
            services.AddScoped<IEmailService, EmailService>();
        }
    }
}
