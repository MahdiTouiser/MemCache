using MemCache.ProductService.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseKestrel(options =>
{
    options.ListenAnyIP(int.TryParse(builder.Configuration["Port"], out int port) ? port : 80);
});

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddData(builder.Configuration["DbConnectionString"] ?? builder.Configuration.GetConnectionString("Default") ?? throw new Exception("Default Connection string cannot be found."));
builder.Services.AddMassTransitWithRabbitMq(builder.Configuration["RabbitMqConfiguration"] ?? throw new Exception("RabbitMq config cannot be found."));

var app = builder.Build();

// Configure the HTTP request pipeline.
await app.MigrateDatabase();

app.UseCors(c =>
{
    c.AllowAnyHeader();
    c.AllowAnyMethod();
    c.AllowAnyOrigin();
});


app.UseAuthorization();

app.MapControllers();

app.Run();
