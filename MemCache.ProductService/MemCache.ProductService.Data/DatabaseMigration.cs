using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MemCache.ProductService.Data.Data;
using MemCache.ProductService.Data.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MemCache.ProductService.Data
{
    public class DatabaseMigration
    {
        private readonly DataDbContext _dataContext;

        public DatabaseMigration(IConfiguration configuration)
        {
            _dataContext = new DateContextFactory().CreateDbContext(configuration);

        }
        public async Task MigrateDatabase()
        {
            Console.WriteLine("Here");
            try
            {
                _dataContext.Database.Migrate();

            }
            catch (Exception)
            {

                throw;
            }
            Console.WriteLine("Here");


            await SeedProducts(_dataContext);
        }

        private static async Task SeedProducts(DataDbContext context)
        {
            if (!context.Products.Any())
            {
                var filePath = Directory.GetCurrentDirectory() + "/Data/data.json";
                using FileStream stream = File.OpenRead(filePath);
                var data = await JsonSerializer.DeserializeAsync<ICollection<JsonDataModel>>(stream, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

                var categories = data!.Select(d => d.Category).Distinct().Select(category => new Category() { Id = Guid.NewGuid(), Name = category });

                await context.Categories.AddRangeAsync(categories);

                await context.SaveChangesAsync();

                await context.Products.AddRangeAsync(data.Select(p => new Product()
                {
                    Description = p.Description,
                    Name = p.Name,
                    Discount = p.Discount,
                    Id = Guid.NewGuid(),
                    Price = p.Price,
                    Quantity = p.Quantity,
                    Category = categories.FirstOrDefault(c => c.Name == p.Category)!
                }));

                await context.SaveChangesAsync();

            }
        }
    }
}
