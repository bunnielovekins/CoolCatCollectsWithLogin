using CoolCatCollects.Bricklink.Models;
using CoolCatCollects.Core;
using CoolCatCollects.Data.Entities;
using CoolCatCollects.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoolCatCollects.Bricklink
{
	/// <summary>
	/// Service to deal with BL-related DB tables
	/// </summary>
	public class BricklinkDataService : IBricklinkDataService
	{
		private readonly IBaseRepository<Part> _partrepo;
		private readonly IPartInventoryRepository _partInventoryRepo;
		private readonly IBaseRepository<PartPriceInfo> _partPricingRepo;
		private readonly IOrderRepository _orderRepo;
		private readonly IBricklinkApiService _api;
		private readonly IBaseRepository<PartInventoryLocationHistory> _historyRepo;
		private readonly IColourService _colourService;

		public BricklinkDataService(IPartInventoryRepository partInventoryRepo, IBaseRepository<Part> partrepo, IBaseRepository<PartPriceInfo> partPricingRepo,
			IOrderRepository orderRepo, IBricklinkApiService api, IBaseRepository<PartInventoryLocationHistory> historyRepo, IColourService colourService)
		{
			_partInventoryRepo = partInventoryRepo;
			_partrepo = partrepo;
			_partPricingRepo = partPricingRepo;
			_historyRepo = historyRepo;
			_colourService = colourService;
			_orderRepo = orderRepo;
			_api = api;
		}

		public Order GetOrder(int orderId)
		{
			return _orderRepo.FindOne(x => x.OrderId == orderId.ToString());
		}

		/// <summary>
		/// Gets a list of all order ids in the database
		/// </summary>
		/// <returns></returns>
		public IEnumerable<string> GetOrderIds()
		{
			return _orderRepo.FindAll().Select(x => x.OrderId);
		}

		private PartModel UpdatedPartModel(PartInventory partInv, bool updateInv = false, bool updatePrice = false, bool updatePart = false,
			DateTime? updateInvDate = null)
		{
			var colourId = partInv.ColourId;
			var condition = partInv.Condition;

			// Get the entities, make sure they're up to date, return them
			if (partInv.LastUpdated < DateTime.Now.AddDays(-1) || updateInv || updateInvDate.HasValue && updateInvDate > partInv.LastUpdated)
			{
				var updatedInv = _api.UpdateInventoryFromApi(partInv);
				if (updatedInv != null)
				{
					partInv = updatedInv;
				}
				else
				{
					partInv.Quantity = 0;
				}
				_partInventoryRepo.Update(partInv);
			}

			var part = partInv.Part;

			if (part.Number == null)
			{
				part = _api.RecoverPartFromPartInv(partInv, part);
				_partrepo.Update(part);
			}
			else if (part.LastUpdated < DateTime.Now.AddDays(-14) || updatePart)
			{
				part = _api.UpdatePartFromApi(part.Number, part.ItemType, part);
				_partrepo.Update(part);
			}

			var pricing = partInv.Pricing;
			if (pricing.LastUpdated < DateTime.Now.AddDays(-14) || updatePrice)
			{
				pricing = _api.UpdatePartPricingFromApi(part.Number, part.ItemType, colourId, condition, pricing);
				_partPricingRepo.Update(pricing);
			}

			return new PartModel
			{
				Part = part,
				PartInventory = partInv,
				PartPriceInfo = pricing
			};
		}

		public PartModel GetPartModel(PartInventory inv, bool updateInv = false, bool updatePrice = false, bool updatePart = false,
			DateTime? updateInvDate = null)
		{
			return GetPartModel(inv.Part.Number, inv.ColourId, inv.Part.ItemType, inv.Condition, updateInv, updatePrice, updatePart, updateInvDate);
		}

		/// <summary>
		/// GetPartModel for when you know the inv id. Can return null.
		/// </summary>
		/// <param name="inventoryId"></param>
		/// <param name="updateInv"></param>
		/// <param name="updatePrice"></param>
		/// <param name="updatePart"></param>
		/// <param name="updateInvDate"></param>
		/// <returns></returns>
		public PartModel GetPartModel(int inventoryId, bool updateInv = false, bool updatePrice = false, bool updatePart = false, DateTime? updateInvDate = null)
		{
			var partInv = _partInventoryRepo.FindOne(x => x.InventoryId == inventoryId);

			if (partInv != null)
			{
				return UpdatedPartModel(partInv, updateInv, updatePrice, updatePart, updateInvDate);
			}

			var model = _api.UpdateInventoryModelFromApi(inventoryId);

			if (model == null)
			{
				return null;
			}

			partInv = model.ToEntity();

			var number = model.Number;
			var type = model.ItemType;
			var colourId = model.ColourId;

			var part = GetPart(number, type);

			var pricing = _api.UpdatePartPricingFromApi(number, type, colourId, partInv.Condition);

			_partInventoryRepo.AddPartInv(ref partInv, ref pricing, ref part);

			return new PartModel
			{
				Part = part,
				PartInventory = partInv,
				PartPriceInfo = pricing
			};
		}

		/// <summary>
		/// Main function, gets a Part/Inventory/Price model for a particular part number/colour/type/condition combination. If it doesn't exist, gets it from the api and creates it.
		/// </summary>
		/// <param name="number">Part Number</param>
		/// <param name="colourId">Colour Id</param>
		/// <param name="type">Type - PART, MINIFIG, etc.</param>
		/// <param name="condition">Condition - N, U, A. A = any, prefers new and falls back to used</param>
		/// <param name="updateInv">Forces an update of the inventory item from the API. By default, it updates once a day</param>
		/// <param name="updatePrice">Forces an update of the price from the API. By default, it updates once every 14 days</param>
		/// <param name="updatePart">Forces an update of the part from the API. By default, it updates once every 14 days</param>
		/// <param name="updateInvDate">Forces an update of the inventory item if it's older than this</param>
		/// <returns>A model with an attached Part, PartInventory, PartPriceInfo</returns>
		public PartModel GetPartModel(string number, int colourId, string type, string condition = "N",
			bool updateInv = false, bool updatePrice = false, bool updatePart = false,
			DateTime? updateInvDate = null, string description = "")
		{
			bool placeholder = false;
			string cond = condition == "A" ? "N" : condition;
			var partInv = _partInventoryRepo.FindOne(x =>
				x.Part.ItemType == type &&
				x.Part.Number == number &&
				x.ColourId == colourId &&
				x.Condition == cond &&
				x.Description == description);

			if (partInv != null)
			{
				return UpdatedPartModel(partInv, updateInv, updatePrice, updatePart, updateInvDate);
			}

			// No part inventory - create it
			var part = GetPart(number, type);

			// Attempt to get the part from the API
			partInv = _api.UpdateInventoryFromApi(type, part.CategoryId, colourId, number, condition == "A" ? "N" : condition, description: description);

			if (partInv == null)
			{
				partInv = PlaceHolderInventory(colourId, condition == "A" ? "N" : condition, description, type, number);
				placeholder = true;
			}

			// Quantity is 0, fall back to used if condition is set to any
			if (partInv.Quantity == 0 && condition == "A")
			{
				var usedPart = _api.UpdateInventoryFromApi(type, part.CategoryId, colourId, number, "U", description: description);
				if (usedPart.Quantity > 0)
				{
					partInv = usedPart;
				}
			}

			if (partInv.ColourId != 0)
			{
				// Lookup colour
				partInv.ColourName = _colourService.Colours[partInv.ColourId].Name;
			}

			var pricing = _api.UpdatePartPricingFromApi(number, type, colourId, partInv.Condition);

			_partInventoryRepo.AddPartInv(ref partInv, ref pricing, ref part);

			return new PartModel
			{
				Part = part,
				PartInventory = partInv,
				PartPriceInfo = pricing,
				InvIsPlaceHolder = placeholder
			};
		}

		private PartInventory PlaceHolderInventory(int colourId, string condition, string description, string type, string number)
		{
			return new PartInventory
			{
				ColourId = colourId,
				InventoryId = 0,
				Quantity = 0,
				MyPrice = 0,
				ColourName = colourId != 0 ? _colourService.Colours[colourId].Name : "",
				Condition = condition,
				Location = "",
				Image = _api.GetItemImage(type, number, colourId),
				Description = description,
				Notes = "",
				LastUpdated = DateTime.Now
			};
		}

		/// <summary>
		/// Gets a part from the DB. If it's out of date or not there, gets it from the API.
		/// </summary>
		/// <param name="number">Part Number</param>
		/// <param name="type">Type - PART, MINIFIG, etc.</param>
		/// <returns>A Part entity</returns>
		public Part GetPart(string number, string type)
		{
			var part = _partrepo.FindOne(x => x.ItemType == type && x.Number == number);

			if (part != null && part.LastUpdated >= DateTime.Now.AddDays(-14))
			{
				return part;
			}

			part = _api.UpdatePartFromApi(number, type, part);
			return part;
		}

		public void AddPartInvFromOrder(PartInventory inv, string no, string type)
		{
			var part = GetPart(no, type);

			var price = _api.UpdatePartPricingFromApi(no, type, inv.ColourId, inv.Condition);

			_partInventoryRepo.AddPartInv(ref inv, ref price, ref part);
		}

		/// <summary>
		/// Adds an empty order to the db
		/// </summary>
		/// <param name="order"></param>
		public int AddInitialOrder(OrderModel order)
		{
			var orderEntity = new Data.Entities.BricklinkOrder
			{
				TotalCount = order.TotalPieces,
				UniqueCount = order.UniquePieces,
				Weight = string.Empty,
				DriveThruSent = false,
				ShippingMethod = string.Empty,
				BuyerRealName = string.Empty,
				BuyerName = order.Username,
				OrderId = order.OrderId.ToString(),
				OrderDate = order.DateOrdered,
				BuyerEmail = string.Empty,
				Subtotal = order.SubTotalDec,
				GrandTotal = order.TotalDec,
				Status = OrderStatus.InProgress,
				OrderItems = new List<OrderItem>()
			};

			return _orderRepo.Add(orderEntity).Id;
		}

		/// <summary>
		/// Adds an order into the DB. If it already exists, return it.
		/// </summary>
		/// <param name="order">Order response model from API</param>
		/// <param name="orderItems">Order items response model from API</param>
		/// <returns>A BL Order entity</returns>
		public Data.Entities.BricklinkOrder AddOrder(GetOrderResponseModel order, GetOrderItemsResponseModel orderItems, int id = 0)
		{
			var orderEntity = (id != 0 ?
				_orderRepo.FindOne(id) :
				_orderRepo.FindOne(x => x.OrderId == order.data.order_id.ToString()))
				as Data.Entities.BricklinkOrder;
			if (orderEntity == null)
			{
				throw new Exception("Order doesn't exist!!"); // todo: do something about this
			}

			if (orderEntity.OrderItems.Any())
			{
				return orderEntity;
			}

			orderEntity.Weight = order.data.total_weight;
			orderEntity.ShippingMethod = order.data.shipping?.method;
			orderEntity.BuyerRealName = order.data.shipping.address.name.full;
			orderEntity.BuyerEmail = order.data.buyer_email;
			orderEntity.Subtotal = decimal.Parse(order.data.cost.subtotal);
			orderEntity.Deductions = decimal.Parse(order.data.cost.credit) + decimal.Parse(order.data.cost.coupon);
			orderEntity.Shipping = decimal.Parse(order.data.cost.shipping);
			orderEntity.ExtraCosts = decimal.Parse(order.data.cost.salesTax) + decimal.Parse(order.data.cost.vat_amount) +
					decimal.Parse(order.data.cost.etc1) + decimal.Parse(order.data.cost.etc2) + decimal.Parse(order.data.cost.insurance);
			orderEntity.GrandTotal = decimal.Parse(order.data.cost.grand_total);
			orderEntity.Status = order.data.status == "CANCELLED" ? OrderStatus.Cancelled : OrderStatus.InProgress;

			var orderItemEntities = orderItems.data
				.SelectMany(x => x)
				.Select(x => new BricklinkOrderItem
				{
					Order = orderEntity,
					Name = x.item.name,
					Quantity = x.quantity,
					UnitPrice = decimal.Parse(x.unit_price_final),
					Part = GetPartInv(x, order)
				}).ToList();

			orderEntity = _orderRepo.AddOrderWithItems(orderEntity, orderItemEntities);

			return _orderRepo.FindOne(x => x.Id == orderEntity.Id) as Data.Entities.BricklinkOrder;

			PartInventory GetPartInv(OrderItemResponseModel item, GetOrderResponseModel ord)
			{
				var model = GetPartModel(item.inventory_id, updateInvDate: order.data.date_ordered);
				if (model != null)
				{
					return model.PartInventory;
				}

				model = GetPartModel(item.item.no, item.color_id, item.item.type, item.new_or_used, description: item.description);
				if (!model.InvIsPlaceHolder)
				{
					return model.PartInventory;
				}

				model = AddInventoryFromOrderItem(item, model);
				return model.PartInventory;
			}
		}

		/// <summary>
		/// An inventory item comes through that we haven't seen yet. An order has taken all its quantity so we can't get it from BL.
		/// Add an inventory item for it.
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		private PartModel AddInventoryFromOrderItem(OrderItemResponseModel model, PartModel partModel)
		{
			var part = partModel.Part;
			var inv = partModel.PartInventory;
			inv.InventoryId = model.inventory_id;
			inv.MyPrice = decimal.Parse(model.unit_price);
			inv.Location = model.remarks;
			inv.LastUpdated = DateTime.Now;



			//var inv = new PartInventory
			//{
			//	InventoryId = model.inventory_id,
			//	Quantity = 0,
			//	MyPrice = decimal.Parse(model.unit_price),
			//	ColourId = model.color_id,
			//	ColourName = model.color_name,
			//	Condition = model.new_or_used,
			//	Location = model.remarks,
			//	Notes = "",
			//	LastUpdated = DateTime.Now,
			//	Part = part,
			//	Image = _api.GetItemImage(model.item.type, model.item.no, model.color_id)
			//};

			if (string.IsNullOrEmpty(inv.Location))
			{
				inv.Description = model.description;
			}

			inv = _partInventoryRepo.Update(inv);

			//var pricing = _api.UpdatePartPricingFromApi(model.item.no, model.item.type, model.color_id, model.new_or_used);

			//_partInventoryRepo.AddPartInv(ref inv, ref pricing, ref part);

			//return new PartModel
			//{
			//	Part = part,
			//	PartInventory = inv,
			//	PartPriceInfo = pricing
			//};

			partModel.PartInventory = inv;
			return partModel;
		}

		public IEnumerable<PartInventoryLocationHistory> GetHistoriesByLocation(string location)
		{
			return _historyRepo.Find(x => x.Location.ToUpper() == location.ToUpper());
		}

		/// <summary>
		/// Orders contain some inventory info, so update what we can from there
		/// </summary>
		/// <param name="inv">The inventory entity</param>
		/// <param name="remarks">BL Remark, becomes location</param>
		/// <param name="unit_price_final">The price this part is listed on BL</param>
		/// <param name="description">Description</param>
		/// <param name="inventory_id">Inventory id</param>
		public void UpdatePartInventoryFromOrder(PartInventory inv, string remarks, string unit_price_final, string description, int inventory_id)
		{
			if (inv == null)
			{
				var model = GetPartModel(inventory_id);
				inv = model.PartInventory;
			}

			if (inv.Quantity == 0)
			{
				inv.Location = "";
				_partInventoryRepo.AddLocationHistory(inv, remarks);
			}
			else
			{
				inv.Location = remarks;
			}

			inv.MyPrice = decimal.Parse(unit_price_final);
			inv.Description = description;

			if (inv.InventoryId == 0)
			{
				inv.InventoryId = inventory_id;
			}

			_partInventoryRepo.Update(inv);
		}
	}
}
