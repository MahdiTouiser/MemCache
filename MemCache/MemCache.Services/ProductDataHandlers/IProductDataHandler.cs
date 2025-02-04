using MemCache.Domain.Models;

namespace MemCache.Services.ProductDataHandlers
{
    public interface IProductDataHandler
    {
        Task<IEnumerable<ProductModel>> GetData();
        void SetNextHandler(IProductDataHandler handler);
        Task DataExpired(string key);

    }
}