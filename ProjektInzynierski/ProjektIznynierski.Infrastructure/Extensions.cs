using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjektIznynierski.Infrastructure.Context;

namespace ProjektIznynierski.Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ProjektInzynierskiDbContext>(options =>
            {
                var connectionString = configuration.GetConnectionString("ProjektInzynierskiDatabase");
                options.UseSqlServer(connectionString);
            });
            return services;
        }
    }
}
