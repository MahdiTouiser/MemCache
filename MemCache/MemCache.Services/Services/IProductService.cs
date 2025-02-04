using MemCache.Domain.Models;

namespace MemCache.Services.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductWithSeller>> GetProducts();
    }
}
