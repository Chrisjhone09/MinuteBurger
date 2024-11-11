using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using System.ComponentModel.DataAnnotations;

namespace MinuteBurger.Models
{
	public class Vouchers
	{
        [Key]
        public string VoucherId { get; set; }
        [Precision(3, 2)]
        public decimal DiscountPercentage { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime ValidUntil { get; set; }
        public int NumberOfAvailability { get; set; }
        public bool isActive { get; set; }
    }
}
