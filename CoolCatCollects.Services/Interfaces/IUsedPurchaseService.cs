using CoolCatCollects.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoolCatCollects.Services
{
	public interface IUsedPurchaseService
	{
		Task Add(UsedPurchaseModel model);
		Task AddBLUpload(UsedPurchaseBLUploadModel model);
		Task Delete(int id);
		Task DeleteBLUpload(int id);
		void Dispose();
		Task Edit(UsedPurchaseModel model);
		Task EditBLUpload(UsedPurchaseBLUploadModel model);
		Task<UsedPurchaseModel> Find(int id);
		Task<UsedPurchaseBLUploadModel> FindBLUpload(int id);
		Task<IEnumerable<UsedPurchaseModel>> GetAll();
		Task<UsedPurchaseModel> GetPurchaseWithContents(int id);
		Task UpdateWeights(int id, IEnumerable<UsedPurchaseWeightModel> weights);
	}
}