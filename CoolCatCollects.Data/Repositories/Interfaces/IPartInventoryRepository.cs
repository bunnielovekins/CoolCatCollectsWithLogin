using CoolCatCollects.Data.Entities;

namespace CoolCatCollects.Data.Repositories
{
	public interface IPartInventoryRepository : IBaseRepository<PartInventory>
	{
		void AddLocationHistory(PartInventory entity, string location);
		void AddPartInv(ref PartInventory inv, ref PartPriceInfo price, ref Part part);
		void CascadeDelete(PartInventory entity);
		PartInventory Update(PartInventory entity);
	}
}