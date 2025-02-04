namespace MemCache.Domain.Entities
{
    public class Seller
    {
        public Seller()
        {
            Id = Guid.NewGuid();
            Products = new List<string>();
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<string> Products { get; set; }
    }
}
