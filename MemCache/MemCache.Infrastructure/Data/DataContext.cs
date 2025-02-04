using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MemCache.Domain.Entities;
using System.Text.Json;

namespace MemCache.Infrastructure.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Seller> Sellers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var productsConverter = new ValueConverter<List<string>, string>(
            v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null), // Convert to JSON string
            v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null)); // Convert back to List<string>

            modelBuilder.Entity<Seller>()
                .Property(s => s.Products)
                .HasConversion(productsConverter);


            base.OnModelCreating(modelBuilder);

        }
    }
}
