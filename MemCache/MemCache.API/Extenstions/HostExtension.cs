using MemCache.Infrastructure.Data;

namespace MemCache.API.Extenstions
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
