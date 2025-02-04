using MassTransit;
using MemCache.Contracts;
using MemCache.Domain.Models;
using MemCache.Infrastructure.ProductWebservices;

namespace MemCache.Services.ProductDataHandlers
{
    public class ProductWebServiceHandler : IProductDataHandler
    {
        private readonly IProductApi _productApi;
        private readonly IPublishEndpoint _publish;

        public ProductWebServiceHandler(IProductApi productApi, IPublishEndpoint publish)
        {
            _productApi = productApi;
            _publish = publish;
        }

        public async Task DataExpired(string key)
        {
            await _publish.Publish(new ProductUpdated(key));
        }

        public async Task<IEnumerable<ProductModel>> GetData()
        {
            var data = await _productApi.GetProductsAsync();
            return data;
        }

        public void SetNextHandler(IProductDataHandler handler)
        {
        }
    }
}