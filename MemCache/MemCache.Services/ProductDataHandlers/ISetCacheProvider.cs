using MemCache.Domain.Models;
using MemCache.Infrastructure.Caching;

namespace MemCache.Services.ProductDataHandlers
{
    public interface ICacheHandlerProvider
    {
        void SetCacheProvider(ICacheProvider provider);
        Task SetCache(string key, IEnumerable<ProductModel>? product);
    }
}
