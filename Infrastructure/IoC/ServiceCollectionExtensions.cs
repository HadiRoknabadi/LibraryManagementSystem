using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Context;


namespace Infrastructures.IoC
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplicationDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("Application_ConnectionString"));
                options.EnableSensitiveDataLogging();
            });
        }

    }
}
