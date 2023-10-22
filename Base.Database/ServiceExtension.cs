using Hyperspan.Base.Database.DbHelpers;
using Hyperspan.Base.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Hyperspan.Base.Database
{
    public static class ServiceExtension
    {

        /// <summary>
        /// Connect DB
        /// </summary>
        /// <param name="serviceCollection">Current Instance of IServiceCollection</param>
        /// <param name="connectionString">Connection string from AppSettings.Json</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddDbConnection(this IServiceCollection serviceCollection, string connectionString)
        {
            serviceCollection.AddDbContext<Contexts>(builder =>
                GetDbContextOptions(builder, connectionString));

            serviceCollection.AddScoped(typeof(IRepository<,,>), typeof(Repository<,,>));
            serviceCollection.AddScoped(typeof(IUnitOfWork<,,>), typeof(UnitOfWork<,,>));

            return serviceCollection;
        }

        /// <summary>
        /// Set Database Context Options
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="connectionString"></param>
        /// <exception cref="Exception">If the connection string passed is either null or empty.</exception>
        private static void GetDbContextOptions(DbContextOptionsBuilder builder,
            string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ApiErrorException(BaseErrorCodes.NullConnectionString);

            builder.UseNpgsql(connectionString: connectionString,
                options =>
                {
                    options.MigrationsHistoryTable("__EFMigrationsHistory", "public");
                });
        }

    }
}
