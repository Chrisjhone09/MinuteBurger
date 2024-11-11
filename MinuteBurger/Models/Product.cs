using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MinuteBurger.Models
{
    public class Product
    {
        
        public int ProductId { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public required string Name { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public required string Description { get; set; }
        [Required (AllowEmptyStrings = false, ErrorMessage = "This field is required")]
        public string Category { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public double Price { get; set; }
        public int StockQuantity { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public required string ImageUrl { get; set; }
        public ICollection<OrderItem> OrderItem { get; set; } 

	}


}
