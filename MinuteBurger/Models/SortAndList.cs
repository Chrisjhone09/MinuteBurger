namespace MinuteBurger.Models
{
    public class SortAndList
    {
        public List<Product> Products { get; set; } = new List<Product>();
        public Product Product { get; set; }
        public string SearchString { get; set; }
    }
}
