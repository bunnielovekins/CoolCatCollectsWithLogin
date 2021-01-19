using CoolCatCollects.Data.Entities;
using System.Collections.Generic;

namespace CoolCatCollects.Data.Repositories
{
	public interface IOrderRepository : IBaseRepository<Order>
	{
		BricklinkOrder AddOrderWithItems(BricklinkOrder orderEntity, List<BricklinkOrderItem> orderItemEntities);
		EbayOrder AddOrderWithItems(EbayOrder orderEntity, List<EbayOrderItem> orderItemEntities);
	}
}