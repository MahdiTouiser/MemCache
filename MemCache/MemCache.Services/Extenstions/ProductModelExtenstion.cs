using Newtonsoft.Json;
using MemCache.Domain.Models;

namespace MemCache.Services.Extenstions
{
    public static class ProductModelExtenstion
    {
        public static IEnumerable<ProductModel> ToProductModels(this string data)
        {
            return JsonConvert.DeserializeObject<IEnumerable<ProductModel>>(data)!;
        }
        public static string AsString(this IEnumerable<ProductModel>? product)
        {
            return product is null ? string.Empty : JsonConvert.SerializeObject(product);
        }
    }
}
