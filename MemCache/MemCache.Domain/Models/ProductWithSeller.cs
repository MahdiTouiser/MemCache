namespace MemCache.Domain.Models
{
    public class ProductWithSeller
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public double Discount { get; set; }
        public int Quantity { get; set; }
        public string Category { get; set; }
        public string Seller { get; set; }
    }
}
