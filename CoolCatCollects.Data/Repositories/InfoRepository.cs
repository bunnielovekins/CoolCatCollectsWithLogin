using CoolCatCollects.Data.Entities;
using System.Linq;

namespace CoolCatCollects.Data.Repositories
{
	public class InfoRepository : BaseRepository<Info>
	{
		public InfoRepository(EfContext context) : base(context)
		{
		}

		public Info GetInfo()
		{
			if (_ctx.Infos.Any())
			{
				return _ctx.Infos.First();
			}

			return Add(new Info());
		}
	}
}
