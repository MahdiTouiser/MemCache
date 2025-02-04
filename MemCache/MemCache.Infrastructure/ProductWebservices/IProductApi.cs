using MemCache.Domain.Models;

namespace MemCache.Infrastructure.ProductWebservices
{
    public interface IProductApi
    {
        Task<IEnumerable<ProductModel>> GetProductsAsync();
    }
}
