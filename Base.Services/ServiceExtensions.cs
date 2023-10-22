using Hyperspan.Base.Services.Auth;
using Microsoft.Extensions.DependencyInjection;

namespace Hyperspan.Base.Services
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddBaseServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            return services;
        }
    }
}