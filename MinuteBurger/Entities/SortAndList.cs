using MinuteBurger.Models;

namespace MinuteBurger.Entities
{
    public class SortAndList
    {
        public List<Product> Products { get; set; } = new List<Product>();
        public List<OrderItem> OrderList { get; set; } = new List<OrderItem>();
        public Product Product { get; set; }
        public string SearchString { get; set; }
    }
}
