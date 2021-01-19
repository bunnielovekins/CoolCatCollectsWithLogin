using CoolCatCollects.Data.Entities;
using CoolCatCollects.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoolCatCollects.Services
{
	public interface ILogService
	{
		Task Add(LogModel model);
		Task Delete(int id);
		void Dispose();
		Task Edit(LogModel model);
		Task<LogModel> FindAsync(int id);
		Task<IEnumerable<LogModel>> GetAll();
		LogModel ToModel(Log log);
	}
}