using MassTransit;
using MemCache.Contracts;
using MemCache.Services.ProductDataHandlers;

namespace MemCache.Services.Consumers
{
    public class ProductUpdatedConsumer : IConsumer<ProductUpdated>
    {
        private readonly IProductDataHandler _productDataHandler;

        public ProductUpdatedConsumer(IProductDataHandler productDataHandler)
        {
            _productDataHandler = productDataHandler;
        }

        public async Task Consume(ConsumeContext<ProductUpdated> context)
        {
            await _productDataHandler.DataExpired(context.Message.key);
        }
    }
}
