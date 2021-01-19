using CoolCatCollects.Ebay.Models;

namespace CoolCatCollects.Ebay
{
	public interface IeBayService
	{
		GetItemModel GetItem(string legacyItemId, string legacyVariationId);
		EbayOrderModel GetOrder(string orderNumber);
		EbayOrdersListModel GetOrders(int limit, int page);
		EbayOrdersListModel GetUnfulfilledOrders();
	}
}