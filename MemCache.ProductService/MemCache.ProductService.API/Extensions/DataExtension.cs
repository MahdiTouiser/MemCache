using Microsoft.EntityFrameworkCore;
using MemCache.ProductService.Data;

namespace MemCache.ProductService.API.Extensions
{
    public static class DataExtension
    {
        public static IServiceCollection AddData(this IServiceCollection services, string dbConnectionString)
        {
            services.AddDbContext<DataDbContext>(opt => opt.UseSqlServer(dbConnectionString));

            services.AddSingleton<DatabaseMigration>();

            return services;
        }
    }
}
