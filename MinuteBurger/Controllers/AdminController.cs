using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinuteBurger.Data;
using MinuteBurger.Models;
using System.Runtime.CompilerServices;

namespace MinuteBurger.Controllers
{
    public class AdminController : Controller
    {

        private readonly OrderingSystemDbContext _context;
        private readonly IWebHostEnvironment _environment;
        public AdminController(OrderingSystemDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var model = await _context.Product.ToListAsync();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AddProduct(Product model)
        {

			var entity = new Product
			{
				Name = model.Name,
				Description = model.Description,
				Price = model.Price,
				StockQuantity = model.StockQuantity,
				ImageUrl = model.ImageUrl,
                Category = model.Category
			};
            _context.Product.Add(model);
            _context.SaveChanges();
            return RedirectToAction("List");
        }
        [HttpGet]
        public IActionResult AddProduct()
        {
            return View();
        }
        [HttpGet]
        public IActionResult EditView(int? id)
        {
            var entity = _context.Product.FirstOrDefault(p=> p.ProductId == id);

            return View(entity);
        }
        [HttpPost]
		public IActionResult Edit(Product model)
		{
            _context.Product.Update(model);
            _context.SaveChanges();
            return RedirectToAction("List");

		}

        [HttpPost, ActionName("EditPost")]
        public IActionResult Delete(int? id)
        {
            var entity = _context.Product.Find(id);

            _context.Product.Remove(entity);
            _context.SaveChanges();
            return RedirectToAction("List");
        }
        [HttpGet]
        public IActionResult VoucherList()
        {
            var listOfVouchers = _context.Voucher.ToList();

            return View(listOfVouchers);
        }
        public IActionResult AddVoucher()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddVoucher(Vouchers model)
        {
            if(model.DiscountPercentage > 100)
            {
                return BadRequest("Discount cannot be greater than 100%");
            } else
            {
				var voucher = new Vouchers
				{
					VoucherId = GenerateRandomVoucherId(),
					DiscountPercentage = model.DiscountPercentage/100,
					ExpirationDate = model.ExpirationDate,
					isActive = model.isActive,
					ValidUntil = model.ValidUntil
				};
				_context.Voucher.Add(voucher);
			}
            _context.SaveChanges();
            return RedirectToAction("VoucherList");
        }
        [HttpPost]
        public IActionResult DeleteVoucher(string id)
        {
            var voucherEntity = _context.Voucher.Find(id);

            _context.Voucher.Remove(voucherEntity);
            _context.SaveChanges();
            return RedirectToAction("VoucherList");
        }
        public IActionResult DeleteVoucher()
        {
            return View();
        }
        public static string GenerateRandomVoucherId()
        {
            Random rand = new Random();

            string prefix = "MB-".ToString();
            string id =rand.Next(999999, 9999999).ToString();
            return $"{prefix}{id}";
        }
    }
}
