using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using MinuteBurger.Data;
using MinuteBurger.Entities;
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
		[HttpPost]
		public IActionResult Index(int[] selectedIds)
		{
			int[] id = selectedIds;
			try
			{
				foreach (var ids in id)
				{
					var item = _context.OrderItem.Find(ids);
					_context.OrderItem.Remove(item);
					_context.SaveChanges();
				}
			}
			catch(Exception e)
			{
				
			}
			return View();
		}

		[HttpGet]
		public IActionResult Index()
		{
			var entities = _context.Product.ToList();


			var ProductsAndOrders = new ProductOrderList
			{
				Products = entities,
				Orders = _context.OrderItem.ToList()
			};

			return View(ProductsAndOrders);
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
		public IActionResult BigTime()
		{
			var item = _context.Product.Where(p=> p.Category == "BigTime").ToList();
			return View(item);
		}
		[HttpGet]
		public IActionResult Beverages()
		{
			var item = _context.Product.Where(p => p.Category == "Beverages").ToList();
			return View(item);
		}
		[HttpGet]
		public IActionResult Burgers()
		{
			var item = _context.Product.Where(p => p.Category == "Burgers").ToList();
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

			return RedirectToAction("Index");
		}


		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}

}
