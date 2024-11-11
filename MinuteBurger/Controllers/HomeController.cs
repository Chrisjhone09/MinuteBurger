using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using MinuteBurger.Data;
using MinuteBurger.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace MinuteBurger.Controllers
{
	public class HomeController : Controller
	{
		private readonly OrderingSystemDbContext _context;

		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger, OrderingSystemDbContext context)
		{
			_logger = logger;
			_context = context;
		}
		[HttpGet]
		public IActionResult Index()
		{
			var entities = _context.Product.ToList();

			return View(entities);
		}
		public IActionResult Privacy()
		{
			return View();
		}
		[HttpGet]
		public IActionResult Item(int id)
		{
			var selectedProduct = _context.Product.Find(id);
			if (selectedProduct == null)
			{
				return NotFound("Product not found.");
			}
			var entity = new OrderItem
			{
				Product = selectedProduct
			};

			return View(entity);
		}
		[HttpPost]
		public IActionResult OrderItem(OrderItem model)
		{
			var selectedProduct = _context.Product.Find(model.Product.ProductId);
			if (selectedProduct == null)
			{
				return NotFound("Product not found.");
			}

			string id = model.VoucherInput;
			bool findExistingVoucher = _context.Voucher.Any(v=> v.VoucherId == model.VoucherInput) ? true : false;
			var getVoucher = _context.Voucher.FirstOrDefault(v=> v.VoucherId == model.VoucherInput);
			if(findExistingVoucher)
			{
				double discount = (double)getVoucher.DiscountPercentage * (model.Quantity * selectedProduct.Price);
				OrderItem entity = new OrderItem
				{
					OrderedAt = DateTime.Now,
					TotalAmount = model.Quantity * selectedProduct.Price - discount,
					Product = selectedProduct,
					Quantity = model.Quantity,
					ProductId = selectedProduct.ProductId
				};
				var order = new Order
				{
					OrderItems = {entity}

				};
				_context.Order.Add(order);
				_context.OrderItem.Add(entity);
			} else
			{
				OrderItem entity = new OrderItem
				{
					OrderedAt = DateTime.Now,
					TotalAmount = model.Quantity * selectedProduct.Price,
					Product = selectedProduct,
					Quantity = model.Quantity,
					ProductId = selectedProduct.ProductId
				};
				
				_context.OrderItem.Add(entity);
			}
			return RedirectToAction("PlaceOrder", entity);
		}
		[HttpGet]
		public IActionResult PlaceOrder(int id)
		{
			var orderItem = _context.OrderItem.
		}
		[HttpGet]
		public IActionResult ShoppingCart()
		{
			var orders = _context.OrderItem.ToList();

			return View(orders);
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
	//is my naming convention bad?

}
