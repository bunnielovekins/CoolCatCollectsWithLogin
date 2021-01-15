﻿using CoolCatCollects.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoolCatCollects.Bricklink.Models
{
	public class OrdersModel
	{
		public OrdersModel()
		{

		}

		public OrdersModel(GetOrdersResponseModel model, string status)
		{
			Orders = model.data.Select(x => new OrderModel(x)).ToList();
			Status = status;
		}

		public List<OrderModel> Orders { get; set; }
		public string Status { get; set; }
	}

	public class OrderModel
	{
		public bool Selected { get; set; }
		public int OrderId { get; set; }
		public int TotalPieces { get; set; }
		public int UniquePieces { get; set; }
		public string Username { get; set; }
		public string RealName { get; set; }
		public string Total { get; set; }
		public string SubTotal { get; set; }
		public decimal TotalDec { get; set; }
		public decimal SubTotalDec { get; set; }
		public DateTime DateStatusChanged { get; set; }
		public DateTime DateOrdered { get; set; }
		public bool OrderIsLoaded { get; set; }
		public int Id { get; set; }
		public string Weight { get; set; }
		public string PackageSize { get; set; }
		public string ShippingMethod { get; set; }
		public bool InternationalOrder { get; set; }
		

		public Day DayToGoOut
		{
			get
			{
				if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday)
				{
					return Day.Monday;
				}
				if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
				{
					return Day.Tomorrow;
				}

				var adjustedDate = DateStatusChanged;//.AddHours(5);

				if (DateTime.Now.Day > adjustedDate.Day || DateTime.Now.Month > adjustedDate.Month)
				{
					return Day.Today;
				}

				if (adjustedDate.Hour >= 12)
				{
					if (DateTime.Now.DayOfWeek == DayOfWeek.Friday)
					{
						return Day.Monday;
					}
					return Day.Tomorrow;
				}

				return Day.Today;
			}
		}

		public OrderModel()
		{

		}

		public OrderModel(GetOrdersResponseModelItem model)
		{
			Selected = true;
			OrderId = model.order_id;
			TotalPieces = model.total_count;
			UniquePieces = model.unique_count;
			Username = model.buyer_name;
			DateStatusChanged = model.date_status_changed;
			DateOrdered = model.date_ordered;
			Total = StaticFunctions.FormatCurrencyStr(model.cost.grand_total);
			SubTotal = StaticFunctions.FormatCurrencyStr(model.cost.subtotal);
			TotalDec = StaticFunctions.FormatCurrency(model.cost.grand_total);
			SubTotalDec = StaticFunctions.FormatCurrency(model.cost.subtotal);
		}

		public enum Day
		{
			Today,
			Tomorrow,
			Monday
		}

		public struct DayToGoOutModel
		{
			public DayToGoOutModel(string description, int ordering)
			{
				Description = description;
				Ordering = ordering;
			}

			public string Description { get; set; }
			public int Ordering { get; set; }

			public override string ToString()
			{
				return Description;
			}
		}
	}
}
