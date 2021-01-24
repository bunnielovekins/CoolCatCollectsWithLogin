using CoolCatCollects.Bricklink.Models;
using CoolCatCollects.Data.Entities;
using System.Collections.Generic;

namespace CoolCatCollects.Models
{
	/// <summary>
	/// Model for the database utility page
	/// </summary>
	public class DatabaseUpdateModel
	{
		public DatabaseUpdateModel()
		{

		}

		public DatabaseUpdateModel(Info info, IEnumerable<ColourModel> colours)
		{
			InventoryLastUpdated = info.InventoryLastUpdated.Year <= 2010 ? "Never" : info.InventoryLastUpdated.ToShortDateString();
			OrdersLastUpdated = info.OrdersLastUpdated.Year <= 2010 ? "Never" : info.OrdersLastUpdated.ToShortDateString();

			Colours = colours;
		}

		public string InventoryLastUpdated { get; set; }
		public string OrdersLastUpdated { get; set; }

		public IEnumerable<ColourModel> Colours { get; set; }
	}
}