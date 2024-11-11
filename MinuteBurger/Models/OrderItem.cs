using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MinuteBurger.Models
{
	public class OrderItem
	{
		public int OrderItemId { get; set; }
		public int ProductId { get; set; }
        public DateTime OrderedAt { get; set; }
        [Required(ErrorMessage = "This field is required")]
		public int Quantity { get; set; }
		public double TotalAmount { get; set; }
		[NotMapped]
        public string? VoucherInput { get; set; }
        public int OrderId { get; set; }
        public Product Product { get; set; }
        public Order Order { get; set; }
    }
}