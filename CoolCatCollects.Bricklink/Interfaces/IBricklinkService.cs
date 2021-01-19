using CoolCatCollects.Bricklink.Models;
using CoolCatCollects.Bricklink.Models.Responses;
using System.Collections.Generic;

namespace CoolCatCollects.Bricklink
{
	public interface IBricklinkService
	{
		IEnumerable<PartModel> GetHistoriesByLocation(string location);
		OrderCsvModel GetOrderForCsv(string orderId);
		IEnumerable<BricklinkMessage> GetOrderMessages(string orderId);
		OrdersModel GetOrders(string status);
		object GetOrdersNotInDb();
		OrderWithItemsModel GetOrderWithItems(string orderId, int id = 0);
		BricklinkItem GetPart(string number, string type = "PART");
		SubsetPartsListModel GetPartsFromSet(string number, bool byRemark = false, bool debug = false, string type = "SET", int colourId = 0);
		object GetSetDetails(string set);
		string LoadOrder(int id, string orderId);
		IEnumerable<string> UpdateInventoryForColour(int colourId);
		void UpdateInventoryForParts(IEnumerable<BricklinkService.MiniPartModel> parts);
	}
}