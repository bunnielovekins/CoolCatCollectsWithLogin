using CoolCatCollects.Data.Entities.Expenses;
using CoolCatCollects.Models.Expenses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoolCatCollects.Services
{
	public interface IExpensesService
	{
		Task Add(ExpenseModel model);
		Task Delete(int id);
		void Dispose();
		Task Edit(ExpenseModel model);
		Task<ExpenseModel> FindAsync(int id);
		Task<IEnumerable<ExpenseModel>> GetAll();
		ExpenseModel ToModel(Expense expense);
	}
}