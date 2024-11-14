using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using MinuteBurger.Data;
using MinuteBurger.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;

namespace MinuteBurger.Controllers
{
	public class HomeController : Controller
	{
		private readonly OrderingSystemDbContext _context;
		private readonly IWebHostEnvironment _webHostEnvironment;

		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger, OrderingSystemDbContext context, IWebHostEnvironment webHostEnvironment)
		{
			_logger = logger;
			_context = context;
			_webHostEnvironment = webHostEnvironment;
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
		[HttpGet]
		public IActionResult BigTimeFilter()
		{
			var item = _context.Product.Where(p=> p.Category == "BigTime").ToList();
			return View(item);
		}
		[HttpPost]
		public IActionResult OrderItem(OrderItem model)
		{
			var selectedProduct = _context.Product.Find(model.Product.ProductId);
			if (selectedProduct == null)
			{
				return NotFound("Product not found.");
			}

			double totalAmount = model.Quantity * selectedProduct.Price;
			var voucher = _context.Voucher.FirstOrDefault(v => v.VoucherId == model.VoucherInput);

			if (voucher != null)
			{
				double discount = (double)(voucher.DiscountPercentage / 100m) * totalAmount;
				totalAmount -= discount;
			}

			OrderItem orderItem = new OrderItem
			{
				OrderedAt = DateTime.Now,
				TotalAmount = totalAmount,
				Product = selectedProduct,
				Quantity = model.Quantity,
				ProductId = selectedProduct.ProductId
			};

			var order = new Order
			{
				OrderItems = new List<OrderItem> { orderItem },
				TotalAmountToPay = orderItem.TotalAmount
			};

			_context.Order.Add(order);
			_context.OrderItem.Add(orderItem);
			_context.SaveChanges();

			return RedirectToAction("PlaceOrder", new { id = orderItem.OrderItemId });
		}

		[HttpGet("Item/{id:int}/PlaceOrder")]
		public IActionResult PlaceOrder(int id)
		{
			var orderItem = _context.OrderItem.Find(id);
			if (orderItem == null)
			{
				return NotFound("Order item not found.");
			}

			var order = new Order
			{
				TotalAmountToPay = orderItem.TotalAmount,
				OrderItems = new List<OrderItem> { orderItem },
				OrderStatus = OrderStatus.Preparing
			};

			return View(order);
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

}
