using Microsoft.EntityFrameworkCore;
using MinuteBurger.Models;

namespace MinuteBurger.Data
{
	public class OrderingSystemDbContext : DbContext
	{
		public OrderingSystemDbContext(DbContextOptions<OrderingSystemDbContext> options) : base(options)
		{

		}
		public DbSet<Product> Product { get; set; }
		public DbSet<OrderItem> OrderItem { get; set; }
		public DbSet<Vouchers> Voucher { get; set; }
		public DbSet<Order> Order { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Vouchers>()
				.HasKey(v => v.VoucherId);

			modelBuilder.Entity<OrderItem>()
				.HasKey(oi => oi.OrderItemId);

			modelBuilder.Entity<Product>()
				.HasKey(p => p.ProductId);

			modelBuilder.Entity<Order>()
				.HasKey(o => o.OrderId);

			modelBuilder.Entity<Order>()
				.HasMany(o => o.OrderItems)
				.WithOne(oi => oi.Order)
				.HasForeignKey(oi => oi.OrderId).OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<Product>()
				.HasMany(p => p.OrderItem)
				.WithOne(oi => oi.Product)
				.HasForeignKey(p => p.ProductId).OnDelete(DeleteBehavior.Cascade);
		}
	}
}
