using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MinuteBurger.Models
{
	public class User
	{
		public int UserId { get; set; }
		[Required(ErrorMessage ="This field is required. Please Enter your User Name")]
		[Unicode (false)]
		public string Username { get; set; }
		[Required(ErrorMessage = "This field is required. Please Enter your Fisrt Name")]
		[Unicode(false)]
		public string  FirstName { get; set; }
		[Required(ErrorMessage = "This field is required. Please Enter your Middle Name")]
		[Unicode(false)]
		public string MiddleName { get; set; }
		[Required(ErrorMessage = "This field is required. Please Enter your Last Name")]
		[Unicode(false)]
		public string LastName { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }
		[DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
		public string PasswordHash { get; set; }
		public DateTime CreatedAt { get; set; }
		public bool IsAdmin { get; set; }
	}
}
