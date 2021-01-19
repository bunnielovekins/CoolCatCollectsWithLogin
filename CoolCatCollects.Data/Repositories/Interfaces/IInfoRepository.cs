using CoolCatCollects.Data.Entities;

namespace CoolCatCollects.Data.Repositories
{
	public interface IInfoRepository : IBaseRepository<Info>
	{
		Info GetInfo();
	}
}