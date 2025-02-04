namespace MemCache.ProductService.Data.Data
{
    internal class JsonDataModel
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public double Price { get; set; }
        public double Discount { get; set; }
        public int Quantity { get; set; }
        public string Category { get; set; }
    }
}
