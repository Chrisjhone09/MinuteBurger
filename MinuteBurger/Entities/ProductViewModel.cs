using MinuteBurger.Models;

namespace MinuteBurger.Entities
{
    public class ProductViewModel
    {
        public Product Product { get; set; }
        public IFormFile Image { get; set; }
    }
}
