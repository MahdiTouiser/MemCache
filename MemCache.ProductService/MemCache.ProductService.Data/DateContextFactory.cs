using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemCache.ProductService.Data
{
    internal class DateContextFactory
    {
        public DataDbContext CreateDbContext(IConfiguration configuration)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DataDbContext>();

            optionsBuilder.UseSqlServer(configuration["DbConnectionString"] ?? configuration.GetConnectionString("Default"));

            return new DataDbContext(optionsBuilder.Options);
        }
    }
}
