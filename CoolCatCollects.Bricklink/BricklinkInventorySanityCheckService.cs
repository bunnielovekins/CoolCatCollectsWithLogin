using CoolCatCollects.Data;
using CoolCatCollects.Data.Entities;
using CoolCatCollects.Data.Repositories;
using CoolCatCollects.Models.Parts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoolCatCollects.Bricklink
{
	public class BricklinkInventorySanityCheckService
	{
		private readonly BricklinkApiService _apiService;
		private readonly BricklinkDataService _dataService;
		private readonly PartInventoryRepository _partInventoryRepo;
		private readonly BaseRepository<Part> _partrepo;
		private readonly BaseRepository<PartPriceInfo> _partPricingRepo;
		private readonly BaseRepository<PartInventoryLocationHistory> _historyRepo;
		private readonly BaseRepository<Order> _orderRepo;
		private readonly BaseRepository<OrderItem> _orderItemRepo;

		public BricklinkInventorySanityCheckService(EfContext context)
		{
			_apiService = new BricklinkApiService(new ColourService(context));
			_dataService = new BricklinkDataService(context);

			_partInventoryRepo = new PartInventoryRepository(context);
			_partrepo = new BaseRepository<Part>(_partInventoryRepo.Context);
			_partPricingRepo = new BaseRepository<PartPriceInfo>(_partInventoryRepo.Context);
			_historyRepo = new BaseRepository<PartInventoryLocationHistory>(_partInventoryRepo.Context);
			_orderRepo = new BaseRepository<Order>(_partInventoryRepo.Context);
			_orderItemRepo = new BaseRepository<OrderItem>(_partInventoryRepo.Context);
		}

		#region inventory

		public IEnumerable<DuplicateInventoryItemsModel> GetDuplicateInventoryItems()
		{
			var dupes = _partInventoryRepo.Queryable().Where(x => x.InventoryId != 0).GroupBy(x => x.InventoryId).Where(x => x.Count() > 1).Take(5).ToList();

			var models = dupes.Select(x => new DuplicateInventoryItemsModel(FromEntity(x.OrderByDescending(y => y.LastUpdated).First()), x.Select(y => y.Id)));

			return models;
		}

		public bool FixDuplicateInventoryItems()
		{
			var dupes = _partInventoryRepo.Queryable().Where(x => x.InventoryId != 0).GroupBy(x => x.InventoryId).Where(x => x.Count() > 1).Take(5).ToList();

			dupes.ForEach(x =>
			{
				var best = x.OrderByDescending(y => y.LastUpdated).First();

				x.Where(y => y.Id != best.Id).ToList().ForEach(y => FixDuplicateInventoryItem(y, best));
			});

			return true;
		}

		public void FixDuplicateInventoryItem(PartInventory inv, PartInventory newInv)
		{
			// Delete price
			var price = inv.Pricing;
			if (price != null && price.Id != newInv.Pricing.Id)
			{
				_partPricingRepo.Remove(price);
			}

			// history - transfer to new inv if not there, otherwise delete
			var locations = inv.LocationHistory;
			locations.ToList().ForEach(FixLocation);

			// order items - point to new inv
			var orderItems = _orderItemRepo.Find(x => x.Part.Id == inv.Id).ToList();
			foreach (var item in orderItems)
			{
				item.Part = newInv;
				_orderItemRepo.Update(item);
			}

			// remove it
			_partInventoryRepo.Remove(inv);

			void FixLocation(PartInventoryLocationHistory loc)
			{
				if (newInv.LocationHistory.Any(x => x.Location == loc.Location))
				{
					_historyRepo.Remove(loc);
				}
				else
				{
					loc.PartInventory = newInv;
					_historyRepo.Update(loc);
				}
			}
		}

		#endregion
		#region parts

		public IEnumerable<object> GetDuplicateParts()
		{
			var dupes = _partrepo.Queryable().GroupBy(x => x.Number).Where(x => x.Count() > 1).Take(5).ToList();

			var models = dupes.Select(x =>
			{
				var first = x.First();
				return new
				{
					number = x.Key,
					type = first.ItemType,
					name = first.Name,
					image = first.ThumbnailUrl,
					count = x.Count()
				};
			});

			return models;
		}

		public void FixDuplicateParts()
		{
			var dupes = _partrepo.Queryable().GroupBy(x => x.Number).Where(x => x.Count() > 1).Take(5).ToList();

			dupes.ForEach(x =>
			{
				var best = x.OrderByDescending(y => y.LastUpdated).First();

				x.Where(y => y.Id != best.Id).ToList().ForEach(y => FixDuplicatePart(y, best));
			});
		}

		public void FixDuplicatePart(Part part, Part newPart)
		{
			part.InventoryItems.ToList().ForEach(item =>
			{
				item.Part = newPart;
				_partInventoryRepo.Update(item);
			});

			_partrepo.Remove(part);
		}

		#endregion
		#region orders

		public IEnumerable<object> GetDuplicateOrders()
		{
			var dupes = _orderRepo.Queryable().GroupBy(x => x.OrderId).Where(x => x.Count() > 1).Take(5).ToList();

			var models = dupes.Select(x => {
				var first = x.First();
				return new
				{
					orderid = x.Key,
					name = first.BuyerName,
					email = first.BuyerEmail,
					items = first.OrderItems.Count,
					count = x.Count()
				};
			});

			return models;
		}

		public void FixDuplicateOrders()
		{
			var dupes = _orderRepo.Queryable().GroupBy(x => x.OrderId).Where(x => x.Count() > 1).Take(5).ToList();

			dupes.ForEach(x =>
			{
				var best = x.First();

				x.Where(y => y.Id != best.Id).ToList().ForEach(y => FixDuplicateOrder(y, best));
			});
		}

		public void FixDuplicateOrder(Order order, Order newOrder)
		{
			order.OrderItems.ToList().ForEach(item =>
			{
				_orderItemRepo.Remove(item);
			});

			_orderRepo.Remove(order);
		}

		#endregion
		#region locations

		public IEnumerable<object> GetDuplicateInventoryLocations()
		{
			var dupes = _partInventoryRepo.Find(x => 
				!string.IsNullOrEmpty(x.Location) && 
				!x.Location.StartsWith("LL") && 
				x.Location != "INSTRUCTIONS" &&
				!x.Location.StartsWith("USED_") &&
				!x.Location.StartsWith("SS") &&
				!x.Location.StartsWith("FILING") &&
				!x.Location.StartsWith("Pukka") &&
				x.Quantity != 0
				)
				.GroupBy(x => x.Location).Where(x => x.Count() > 1).Take(5).ToList();

			var models = dupes.Select(x => new 
			{ 
				location = x.Key,
				items = x.Select(FromEntity)
			});

			return models;
		}

		#endregion
		#region old inventory

		public int GetOldInventory()
		{
			var aMonthAgo = DateTime.Now.AddMonths(-1);
			var oldOnes = _partInventoryRepo.Find(x => x.LastUpdated < aMonthAgo && x.Quantity != 0).Count();

			return oldOnes;
		}

		public int FixOldInventory()
		{
			var aMonthAgo = DateTime.Now.AddMonths(-1);
			var olds = _partInventoryRepo.Find(x => x.LastUpdated < aMonthAgo && x.Quantity != 0).Take(5).ToList();
			var count = 0;

			olds.ForEach(inv => {
				var model = _dataService.GetPartModel(inv.InventoryId);
				if (model != null)
				{
					count++;
					return;
				}
				model = _dataService.GetPartModel(inv);
			});

			return count;
		}

		#endregion


		public PartInventoryModel FromEntity(PartInventory inv)
		{
			return new PartInventoryModel
			{
				Id = inv.Id,
				InventoryId = inv.InventoryId,
				ColourId = inv.ColourId,
				ColourName = inv.ColourName,
				Condition = inv.Condition,
				Location = inv.Location,
				Image = inv.Image,
				LastUpdated = inv.LastUpdated,
				Number = inv.Part.Number,
				Name = inv.Part.Name,
				ItemType = inv.Part.ItemType,
				CategoryId = inv.Part.CategoryId,
				Quantity = inv.Quantity
			};
		}

		public struct DuplicateInventoryItemsModel
		{
			public DuplicateInventoryItemsModel(PartInventoryModel inv, IEnumerable<int> ids)
			{
				Inventory = inv;
				Ids = ids;
			}

			public PartInventoryModel Inventory { get; set; }
			public IEnumerable<int> Ids { get; set; }
		}
	}
}
