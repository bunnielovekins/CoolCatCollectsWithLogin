using CoolCatCollects.Data.Entities.Purchases;
using CoolCatCollects.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoolCatCollects.Services
{
	public interface INewPurchaseService
	{
		Task Add(NewPurchaseModel model);
		Task Delete(int id);
		void Dispose();
		Task Edit(NewPurchaseModel model);
		Task<NewPurchaseModel> FindAsync(int id);
		Task<IEnumerable<NewPurchaseModel>> GetAll();
		NewPurchaseModel ToModel(NewPurchase newPurchase);
	}
}