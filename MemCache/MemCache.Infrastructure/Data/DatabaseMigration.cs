using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MemCache.Domain.Entities;
using MemCache.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MemCache.Infrastructure.Data
{
    public class DatabaseMigration
    {
        private readonly DataContext _dataContext;

        public DatabaseMigration(IConfiguration configuration)
        {
            _dataContext = new DateContextFactory().CreateDbContext(configuration);

        }
        public async Task MigrateDatabase()
        {
            _dataContext.Database.Migrate();


            await SeedProducts(_dataContext);
        }

        private static async Task SeedProducts(DataContext context)
        {
            if (!context.Sellers.Any())
            {
                var filePath = Directory.GetCurrentDirectory() + "/Data/data.json";
                using FileStream stream = File.OpenRead(filePath);
                var data = await JsonSerializer.DeserializeAsync<ICollection<JsonDataModel>>(stream, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

                await context.Sellers.AddRangeAsync(data.Select(p => new Seller()
                {
                    Name = p.Name,
                    Products = p.Products
                }));

                await context.SaveChangesAsync();

            }
        }
    }
}
