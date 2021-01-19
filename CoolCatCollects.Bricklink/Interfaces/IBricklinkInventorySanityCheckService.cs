using CoolCatCollects.Data.Entities;
using CoolCatCollects.Models.Parts;
using System.Collections.Generic;

namespace CoolCatCollects.Bricklink
{
	public interface IBricklinkInventorySanityCheckService
	{
		void FixDuplicateInventoryItem(PartInventory inv, PartInventory newInv);
		bool FixDuplicateInventoryItems();
		void FixDuplicateOrder(Order order, Order newOrder);
		void FixDuplicateOrders();
		void FixDuplicatePart(Part part, Part newPart);
		void FixDuplicateParts();
		int FixOldInventory();
		PartInventoryModel FromEntity(PartInventory inv);
		IEnumerable<BricklinkInventorySanityCheckService.DuplicateInventoryItemsModel> GetDuplicateInventoryItems();
		IEnumerable<object> GetDuplicateInventoryLocations();
		IEnumerable<object> GetDuplicateOrders();
		IEnumerable<object> GetDuplicateParts();
		int GetOldInventory();
	}
}