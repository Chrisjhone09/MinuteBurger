﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MinuteBurger.Models
{
	public class Order
	{
		[Key]
		public int OrderId { get; set; }
		public OrderStatus OrderStatus { get; set; }
		public double TotalAmountToPay { get; set; }
		public ICollection<OrderItem> OrderItems { get; set; } = [];
	}
	public enum OrderStatus
	{
		Delivered,
		OutForDelivery,
		Preparing
	}
}