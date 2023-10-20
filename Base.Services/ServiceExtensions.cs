using Base.Database;
using Base.Services.Auth;
using Microsoft.Extensions.DependencyInjection;

namespace Base.Services
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddBaseServices(this IServiceCollection services, string connectionString)
        {
            services.AddDbConnection(connectionString);
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            return services;
        }
    }
}