using CoolCatCollects.Bricklink.Models;
using CoolCatCollects.Data.Entities;
using System;
using System.Collections.Generic;

namespace CoolCatCollects.Bricklink
{
	public interface IBricklinkDataService
	{
		int AddInitialOrder(OrderModel order);
		Data.Entities.BricklinkOrder AddOrder(GetOrderResponseModel order, GetOrderItemsResponseModel orderItems, int id = 0);
		void AddPartInvFromOrder(PartInventory inv, string no, string type);
		IEnumerable<PartInventoryLocationHistory> GetHistoriesByLocation(string location);
		Order GetOrder(int orderId);
		IEnumerable<string> GetOrderIds();
		Part GetPart(string number, string type);
		PartModel GetPartModel(int inventoryId, bool updateInv = false, bool updatePrice = false, bool updatePart = false, DateTime? updateInvDate = null);
		PartModel GetPartModel(PartInventory inv, bool updateInv = false, bool updatePrice = false, bool updatePart = false, DateTime? updateInvDate = null);
		PartModel GetPartModel(string number, int colourId, string type, string condition = "N", bool updateInv = false, bool updatePrice = false, bool updatePart = false, DateTime? updateInvDate = null, string description = "");
		void UpdatePartInventoryFromOrder(PartInventory inv, string remarks, string unit_price_final, string description, int inventory_id);
	}
}