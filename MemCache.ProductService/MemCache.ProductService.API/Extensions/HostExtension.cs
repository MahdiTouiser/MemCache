using Microsoft.AspNetCore.Identity;
using MemCache.ProductService.Data;

namespace MemCache.ProductService.API.Extensions
{
    public static class HostExtension
    {
        public static async Task<IHost> MigrateDatabase(this IHost host)
        {
            using (var serviceScope = host.Services.CreateScope())
            {
                var migrationService = serviceScope.ServiceProvider.GetRequiredService<DatabaseMigration>();
                await migrationService.MigrateDatabase();
            }

            return host;
        }
    }
}
