using Microsoft.AspNetCore.Identity.UI;
using MinuteBurger.Models;

namespace MinuteBurger.Entities
{
    public class ProductOrderList
    {
        public List<Product> Products { get; set; } = new List<Product>();
        public List<OrderItem> Orders { get; set; } = new List<OrderItem>();
    }
}
