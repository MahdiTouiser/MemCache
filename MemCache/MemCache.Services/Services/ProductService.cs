using MemCache.Domain.Models;
using MemCache.Infrastructure.Data;
using MemCache.Services.ProductDataHandlers;

namespace MemCache.Services.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductDataHandler _productDataHandler;
        private readonly DataContext _dataContext;

        public ProductService(IProductDataHandler productDataHandler, DataContext dataContext)
        {
            _productDataHandler = productDataHandler;
            _dataContext = dataContext;
        }

        public async Task<IEnumerable<ProductWithSeller>> GetProducts()
        {
            var sellers = _dataContext.Sellers.ToList();
            var products = await _productDataHandler.GetData();

            var productsWithSeller = new List<ProductWithSeller>();

            foreach (var item in products)
            {
                var productSellers = sellers.Where(s => s.Products.Contains(item.Name));
                foreach (var seller in productSellers)
                    productsWithSeller.Add(new ProductWithSeller()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Category = item.CategoryName,
                        Description = item.Description,
                        Discount = item.Discount,
                        Price = item.Price,
                        Quantity = item.Quantity,
                        Seller = seller.Name
                    });
            }

            return productsWithSeller;
        }
    }
}
