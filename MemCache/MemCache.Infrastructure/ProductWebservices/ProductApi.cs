using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using MemCache.Domain.Models;

namespace MemCache.Infrastructure.ProductWebservices
{
    public class ProductApi : IProductApi
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly string _baseUrl;

        public ProductApi(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;

            _baseUrl = _configuration["ProductApiUrl"]!;
        }

        public async Task<IEnumerable<ProductModel>> GetProductsAsync()
        {
            string apiUrl = $"{_baseUrl}/api/products";

            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            if(content == "null")
                return null;

            return JsonConvert.DeserializeObject<IEnumerable<ProductModel>>(content)!;
        }
    }
}
