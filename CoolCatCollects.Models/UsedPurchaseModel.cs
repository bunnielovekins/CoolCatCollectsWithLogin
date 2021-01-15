﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CoolCatCollects.Models
{
	public class UsedPurchaseModel
	{
		public int Id { get; set; }
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
		public DateTime Date { get; set; }
		public string Source { get; set; }
		[DisplayName("Source Username")]
		public string SourceUsername { get; set; }
		[DisplayName("Order Number")]
		public string OrderNumber { get; set; }
		[DisplayName("Price Paid")]
		[DataType(DataType.Currency)]
		public string Price { get; set; }
		[DisplayName("Payment Method")]
		public string PaymentMethod { get; set; }
		public bool Receipt { get; set; }
		[DisplayName("Distance Travelled")]
		public string DistanceTravelled { get; set; }
		public string Location { get; set; }
		public decimal Postage { get; set; }
		[DisplayName("Weight (KG)")]
		public decimal Weight { get; set; }
		[DisplayName("Price per Kilo (£)")]
		[DataType(DataType.Currency)]
		public decimal PricePerKilo { get; set; }
		[DisplayName("Complete Sets?")]
		public bool CompleteSets { get; set; }
		[DataType(DataType.MultilineText)]
		public string Notes { get; set; }
		[DisplayName("Bundle Weight")]
		public decimal TotalBundleWeight { get; set; }

		public IEnumerable<UsedPurchaseWeightModel> Weights { get; set; }
		public IEnumerable<UsedPurchaseBLUploadModel> BLUploads { get; set; }
	}

	public class UsedPurchaseWeightModel
	{
		public int Id { get; set; }
		public string Colour { get; set; }
		[DisplayName("Weight (g)")]
		public decimal Weight { get; set; }
		public int UsedPurchaseId { get; set; }

		public bool Delete { get; set; }
	}

	public class UsedPurchaseBLUploadModel
	{
		public int Id { get; set; }
		[DisplayName("Upload Date")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
		public DateTime Date { get; set; }
		[DisplayName("Number of Parts")]
		public int Parts { get; set; }
		[DisplayName("Number of Lots")]
		public int Lots { get; set; }
		[DisplayName("Total Value")]
		public decimal Value { get; set; }
		[DataType(DataType.MultilineText)]
		public string Notes { get; set; }
		public int UsedPurchaseId { get; set; }
	}
}
