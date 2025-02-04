using MassTransit;
using Microsoft.Extensions.Configuration;
using MemCache.Domain.Models;
using MemCache.Infrastructure.Caching;
using MemCache.Services.Extenstions;

namespace MemCache.Services.ProductDataHandlers
{
    public class InMemoryCacheHandler : IProductDataHandler, ICacheHandlerProvider
    {
        private const string CacheKey = "Products";
        private IProductDataHandler _nextHandler;
        private ICacheProvider _cacheProvider;
        private readonly IConfiguration _configuration;
        public InMemoryCacheHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IEnumerable<ProductModel>> GetData()
        {
            var data = await _cacheProvider.Get(CacheKey);
            if(data == "0")
                return null;

            IEnumerable<ProductModel> products = new List<ProductModel>();

            if (string.IsNullOrEmpty(data))
                products = await _nextHandler.GetData();
            else
                return data.ToProductModels();

            await SetCache(CacheKey, products);

            return products;
        }

        public void SetNextHandler(IProductDataHandler handler)
        {
            _nextHandler = handler;
        }

        public void SetCacheProvider(ICacheProvider provider)
        {
            _cacheProvider = provider;
        }

        public async Task SetCache(string key, IEnumerable<ProductModel>? products)
        {
            if (products is not null)
                await _cacheProvider.Set(key.ToString(), products.AsString(), int.Parse(_configuration["CacheConfiguration:InMemoryCacheExpirationTime"]!));
            else
                await _cacheProvider.Set(key.ToString(), "0", int.Parse(_configuration["CacheConfiguration:MissDataExpirationTime"]!)); //Preventing Attack
        }

        public async Task DataExpired(string key)
        {
            await _cacheProvider.Clear(CacheKey);
        }
    }

}
