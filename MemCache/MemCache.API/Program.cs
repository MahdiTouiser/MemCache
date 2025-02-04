using Microsoft.Extensions.Caching.Memory;
using MemCache.API.Extenstions;
using MemCache.Infrastructure.ProductWebservices;
using MemCache.Infrastructure.Caching;
using MemCache.Services.ProductDataHandlers;
using MemCache.Services.Services;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseKestrel(options =>
{
    options.ListenAnyIP(int.TryParse(builder.Configuration["Port"], out int port) ? port : 80);
});

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddHttpClient<IProductApi, ProductApi>();


builder.Services.AddMemoryCache();
builder.Services.AddSingleton<ICacheProvider, InMemoryCacheProvider>();

builder.Services.AddSingleton<InMemoryCacheHandler>();
builder.Services.AddScoped<ProductWebServiceHandler>();
builder.Services.AddSingleton<InMemoryCacheProvider>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services
    .AddData(builder.Configuration["DbConnectionString"] ?? builder.Configuration.GetConnectionString("Default") ?? throw new Exception("Default Connection string cannot be found."))
    .AddProductChainHandler()
    .AddMassTransitWithRabbitMq();

var app = builder.Build();

await app.MigrateDatabase();

app.UseCors(c =>
{
    c.AllowAnyHeader();
    c.AllowAnyMethod();
    c.AllowAnyOrigin();
});


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
