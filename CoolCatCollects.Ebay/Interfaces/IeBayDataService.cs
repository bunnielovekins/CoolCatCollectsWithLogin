using CoolCatCollects.Data.Entities;
using CoolCatCollects.Ebay.Models;
using CoolCatCollects.Ebay.Models.Responses;

namespace CoolCatCollects.Ebay
{
	public interface IeBayDataService
	{
		EbayOrder AddOrder(GetOrderResponseModel obj);
		void UpdateOrderItemsByLegacyId(string legacyItemId, string legacyVariationId, GetItemModel model);
	}
}