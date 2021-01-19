using CoolCatCollects.Data.Entities;
using CoolCatCollects.Models.Parts;

namespace CoolCatCollects.Bricklink
{
	public interface IBricklinkApiService
	{
		string GetItemImage(string type, string number, int colourId);
		string GetRequest(string url);
		T GetRequest<T>(string url) where T : class;
		Part RecoverPartFromPartInv(PartInventory partInv, Part part);
		PartInventory UpdateInventoryFromApi(int inventoryId, PartInventory partInv = null);
		PartInventory UpdateInventoryFromApi(PartInventory partInv);
		PartInventory UpdateInventoryFromApi(string type, int categoryId, int colourId, string number, string condition = "N", PartInventory partInv = null, string description = "");
		PartInventoryModel UpdateInventoryModelFromApi(int inventoryId);
		Part UpdatePartFromApi(string number, string type, Part part = null);
		PartPriceInfo UpdatePartPricingFromApi(string number, string type, int colourId, string condition = "N", PartPriceInfo price = null);
	}
}