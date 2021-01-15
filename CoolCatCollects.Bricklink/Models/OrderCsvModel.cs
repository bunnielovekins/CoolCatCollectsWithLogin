using CoolCatCollects.Core;
using System.ComponentModel;

namespace CoolCatCollects.Bricklink.Models
{
	/// <summary>
	/// Model for making into the Royal mail CSV
	/// </summary>
	public class OrderCsvModel
	{
		public OrderCsvModel()
		{

		}

		public OrderCsvModel(GetOrderResponseModel model)
		{
			var data = model.data;

			if (data.shipping == null)
			{
				Name = "ERROR: Shipping was null!";
				return;
			}

			if (data.shipping.address == null)
			{
				Name = "ERROR: Shipping address was null!";
				return;
			}

			Name = data.shipping.address.name.full.HtmlDecode();
			Address1 = data.shipping.address.address1.HtmlDecode();
			Address2 = data.shipping.address.address2.HtmlDecode();
			AddressCity = data.shipping.address.city.HtmlDecode();
			AddressPostcode = data.shipping.address.postal_code.HtmlDecode();
			AddressCounty = data.shipping.address.state.HtmlDecode();
			AddressCountry = data.shipping.address.country_code;
			OrderReference = data.order_id.ToString();
			OrderValue = StaticFunctions.FormatCurrency(data.cost?.grand_total ?? "0").ToString();
			ShippingCost = StaticFunctions.FormatCurrency(data.cost?.shipping ?? "0").ToString();
			Weight = PostageHelper.FormatWeight(data.total_weight);
			ServiceCode = PostageHelper.GetServiceCode(data.shipping.method);
			PackageSize = PostageHelper.GetPackageSize(data.shipping.method);

			ProductName = "Mixed Lego (No Batteries)";
			UnitPrice = StaticFunctions.FormatCurrency(data.cost?.subtotal ?? "0").ToString();
			Quantity = "1";
			UnitWeight = Weight;
			//EmailAddress = data.buyer_email;
			//EmailNotification = !string.IsNullOrEmpty(data.buyer_email);
			CountryOfOrigin = "Denmark";
		}

		[DisplayName("Name")]
		public string Name { get; set; }
		[DisplayName("Address Line 1")]
		public string Address1 { get; set; }
		[DisplayName("Address Line 2")]
		public string Address2 { get; set; }
		[DisplayName("Address City")]
		public string AddressCity { get; set; }
		[DisplayName("Address Postcode")]
		public string AddressPostcode { get; set; }
		[DisplayName("Address County")]
		public string AddressCounty { get; set; }
		[DisplayName("Address Country")]
		public string AddressCountry { get; set; }
		[DisplayName("Order Reference")]
		public string OrderReference { get; set; }
		[DisplayName("Order Value")]
		public string OrderValue { get; set; }
		[DisplayName("Shipping Cost")]
		public string ShippingCost { get; set; }
		[DisplayName("Weight")]
		public string Weight { get; set; }
		public string PackageSize { get; set; }
		[DisplayName("Product Name")]
		public string ProductName { get; set; }
		[DisplayName("Unit Price")]
		public string UnitPrice { get; set; }
		public string Quantity { get; set; }
		[DisplayName("Unit Weight")]
		public string UnitWeight { get; set; }
		[DisplayName("Email Address")]
		//public string EmailAddress { get; set; }
		//public bool EmailNotification { get; set; }
		public string CountryOfOrigin { get; set; }
		public string ServiceCode { get; set; }
	}
}
