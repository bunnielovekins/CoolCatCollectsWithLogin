﻿using CoolCatCollects.Bricklink;
using CoolCatCollects.Bricklink.Models;
using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web.Mvc;

namespace CoolCatCollects.Controllers
{
	public class BricklinkPostageController : BaseController
	{
		private readonly BricklinkService _service;

		public BricklinkPostageController()
		{
			_service = new BricklinkService(DbContext);
		}

		public ActionResult Index()
		{
			return RedirectToAction("List");
		}

		public ActionResult List(string overrideStage = "PACKED")
		{
			var result = _service.GetOrders(overrideStage);

			return View(result);
		}

		public ActionResult Export(IEnumerable<OrderModel> orders)
		{
			var ordersWithShipping = orders
				.Where(x => x.Selected)
				.Select(x => {
					var order = _service.GetOrderForCsv(x.OrderId.ToString());
					order.Weight = x.Weight;
					order.ServiceCode = x.ShippingMethod;
					order.PackageSize = x.PackageSize;
					return order;
				})
				.OrderBy(x => x.ServiceCode)
				.ThenBy(x => x.Weight)
				.ThenBy(x => x.Name);

			var bytes = WriteCsvToMemory(ordersWithShipping);

			return File(bytes, "text/csv", DateTime.Now.ToString("yyyy-MM-dd") + " Bricklink Export.csv");
		}

		public byte[] WriteCsvToMemory(IEnumerable<OrderCsvModel> records)
		{
			using (var memoryStream = new MemoryStream())
			using (var streamWriter = new StreamWriter(memoryStream))
			using (var csvWriter = new CsvWriter(streamWriter, new CsvConfiguration(Thread.CurrentThread.CurrentCulture)))
			{
				csvWriter.WriteRecords(records);
				streamWriter.Flush();
				return memoryStream.ToArray();
			}
		}
	}
}