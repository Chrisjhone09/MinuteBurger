using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinuteBurger.Data;
using MinuteBurger.Models;
using System.Runtime.CompilerServices;

namespace MinuteBurger.Controllers
{
    /// <summary>
    /// AdminController handles administrative actions for managing products and vouchers.
    /// </summary>
    public class AdminController : Controller
    {
        private readonly OrderingSystemDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminController(OrderingSystemDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _webHostEnvironment = environment;
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var model = await _context.Product.ToListAsync();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductViewModel model)
        {
            if (model.Image != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                Directory.CreateDirectory(uploadsFolder);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.Image.CopyToAsync(fileStream);
                }

                model.Product.ImageUrl = "/uploads/" + uniqueFileName;
            }

            var entity = new Product
            {
                Name = model.Product.Name,
                Description = model.Product.Description,
                Price = model.Product.Price,
                StockQuantity = model.Product.StockQuantity,
                ImageUrl = model.Product.ImageUrl,
                Category = model.Product.Category
            };
            _context.Product.Add(entity);
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
            var entity = _context.Product.FirstOrDefault(p => p.ProductId == id);

            var productViewModel = new ProductViewModel
            {
                Product = entity
            };
            return View(productViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductViewModel model)
        {
            if (model.Image != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                Directory.CreateDirectory(uploadsFolder);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.Image.CopyToAsync(fileStream);
                }

                model.Product.ImageUrl = "/uploads/" + uniqueFileName;
            }

            var entity = _context.Product.Find(model.Product.ProductId);

            entity.Name = model.Product.Name;
            entity.Description = model.Product.Description;
            entity.Price = model.Product.Price;
            entity.StockQuantity = model.Product.StockQuantity;
            entity.ImageUrl = model.Product.ImageUrl;
            entity.Category = model.Product.Category;

            _context.Product.Update(entity);
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
            if (model.DiscountPercentage > 100)
            {
                return BadRequest("Discount cannot be greater than 100%");
            }
            else
            {
                var voucher = new Vouchers
                {
                    VoucherId = GenerateRandomVoucherId(),
                    DiscountPercentage = model.DiscountPercentage / 100,
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
            string prefix = "MB-";
            string id = rand.Next(999999, 9999999).ToString();
            return $"{prefix}{id}";
        }
    }
}
