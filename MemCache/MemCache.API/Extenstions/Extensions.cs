using MassTransit;
using MemCache.Infrastructure.ProductWebservices;
using MemCache.Infrastructure.Caching;
using MemCache.Services.ProductDataHandlers;
using MemCache.Services.Consumers;
using MemCache.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MemCache.API.Extenstions
{
    public static class Extensions
    {
        public static IServiceCollection AddMassTransitWithRabbitMq(this IServiceCollection services)
        {
            services.AddMassTransit(opts =>
            {
                opts.AddConsumer<ProductUpdatedConsumer>();
                opts.UsingRabbitMq((context, configurator) =>
                {
                    var config = context.GetService<IConfiguration>();

                    configurator.Host(config["RabbitMqConfiguration"]);
                    configurator.ConfigureEndpoints(context, KebabCaseEndpointNameFormatter.Instance);
                    configurator.UseMessageRetry(retryConfigurator =>
                    {
                        retryConfigurator.Interval(3, TimeSpan.FromSeconds(5));
                    });
                });
            });

            return services;
        }

        public static IServiceCollection AddProductChainHandler(this IServiceCollection services)
        {
            services.AddScoped<IProductDataHandler>(serviceProvider =>
            {
                var inMemoryCacheProvider = serviceProvider.GetRequiredService<InMemoryCacheProvider>();
                var productApi = serviceProvider.GetRequiredService<IProductApi>();

                var productWebServiceHandler = serviceProvider.GetRequiredService<ProductWebServiceHandler>();
                var inMemoryCacheHandler = serviceProvider.GetRequiredService<InMemoryCacheHandler>();

                inMemoryCacheHandler.SetCacheProvider(inMemoryCacheProvider);

                inMemoryCacheHandler.SetNextHandler(productWebServiceHandler);

                // Return the first handler in the chain
                return inMemoryCacheHandler;
            });

            return services;
        }

        public static IServiceCollection AddData(this IServiceCollection services, string dbConnectionString)
        {
            services.AddDbContext<DataContext>(opt => opt.UseSqlServer(dbConnectionString));

            services.AddSingleton<DatabaseMigration>();

            return services;
        }

    }
}
