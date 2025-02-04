using MassTransit;

namespace MemCache.ProductService.API.Extensions
{
    public static class MassTransitExtension
    {
        public static IServiceCollection AddMassTransitWithRabbitMq(this IServiceCollection services, string config)
        {
            services.AddMassTransit(opts =>
            {
                opts.UsingRabbitMq((context, configurator) =>
                {

                    configurator.Host(config);
                    configurator.ConfigureEndpoints(context, KebabCaseEndpointNameFormatter.Instance);
                    configurator.UseMessageRetry(retryConfigurator =>
                    {
                        retryConfigurator.Interval(3, TimeSpan.FromSeconds(5));
                    });
                });
            });

            return services;
        }
    }
}
