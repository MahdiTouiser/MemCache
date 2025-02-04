using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MemCache.Infrastructure.Data
{
    internal class DateContextFactory
    {
        public DataContext CreateDbContext(IConfiguration configuration)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();

            optionsBuilder.UseSqlServer(configuration["DbConnectionString"] ?? configuration.GetConnectionString("Default"));

            return new DataContext(optionsBuilder.Options);
        }
    }
}
