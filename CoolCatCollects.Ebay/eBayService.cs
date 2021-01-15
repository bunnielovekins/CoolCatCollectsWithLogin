﻿using CoolCatCollects.Data;
using CoolCatCollects.Data.Entities;
using CoolCatCollects.Ebay.Models;
using CoolCatCollects.Ebay.Models.Responses;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace CoolCatCollects.Ebay
{
	/// <summary>
	/// Top-level service for everything ebay related
	/// </summary>
	public class eBayService
	{
		private readonly eBayApiService _service;
		private readonly eBayDataService _dataService;

		public eBayService(EfContext context)
		{
			_service = new eBayApiService();
			_dataService = new eBayDataService(context);
		}

		/// <summary>
		/// Gets an order, also loads it into the DB
		/// </summary>
		/// <param name="orderNumber">ebay order number</param>
		/// <returns></returns>
		public EbayOrderModel GetOrder(string orderNumber)
		{
			var response = _service.GetRequest($"sell/fulfillment/v1/order/{orderNumber}");

			var obj = JsonConvert.DeserializeObject<GetOrderResponseModel>(response);

			var orderEntity = _dataService.AddOrder(obj);

			bool updated = false;
			foreach (var item in orderEntity.OrderItems.Cast<EbayOrderItem>())
			{
				if (!string.IsNullOrEmpty(item.Image))
				{
					continue;
				}

				updated = true;

				var itemFromApi = GetItem(item.LegacyItemId, item.LegacyVariationId);

				item.Image = itemFromApi.Image;
				item.CharacterName = itemFromApi.Character;
			}

			if (updated)
			{
				orderEntity = _dataService.AddOrder(obj);
			}

			return new EbayOrderModel(obj, orderEntity);
		}

		/// <summary>
		/// Gets a page of orders
		/// </summary>
		/// <param name="limit">How many to return per page</param>
		/// <param name="page">Page number</param>
		/// <returns></returns>
		public EbayOrdersListModel GetOrders(int limit, int page)
		{
			//$"?filter=orderfulfillmentstatus:{{{status}}}" + {FULFILLED|IN_PROGRESS} {NOT_STARTED|IN_PROGRESS}
			int offset = (page - 1) * 50;
			var response = _service.GetRequest("/sell/fulfillment/v1/order" +
				$"?limit={limit}" +
				$"&offset={offset}");

			var obj = JsonConvert.DeserializeObject<GetOrdersResponseModel>(response);

			var items = obj.orders.Select(data =>
			{
				var mod = new EbayOrdersListItemModel(data);
				var entity = _dataService.AddOrder(data);

				mod.Items = mod.Items.Select(x =>
				{
					var ent = entity.OrderItems.Cast<EbayOrderItem>().FirstOrDefault(y => y.LegacyItemId == x.LegacyItemId && y.LineItemId == x.LineItemId && y.LegacyVariationId == x.LegacyVariationId);

					if (ent == null)
					{
						return x;
					}

					x.OrderItemId = ent.Id;
					if (!string.IsNullOrEmpty(ent.Image))
					{
						x.Image = ent.Image;
						x.Character = ent.CharacterName;
					}

					return x;
				});

				return mod;
			});

			var model = new EbayOrdersListModel(items, page, limit);

			return model;
		}

		public EbayOrdersListModel GetUnfulfilledOrders()
		{
			var response = _service.GetRequest("/sell/fulfillment/v1/order" +
				"?limit=1000&filter=orderfulfillmentstatus:{NOT_STARTED | IN_PROGRESS}");

			var obj = JsonConvert.DeserializeObject<GetOrdersResponseModel>(response);

			var items = obj.orders.Where(x => x.cancelStatus.cancelState == "NONE_REQUESTED" && x.orderPaymentStatus == "PAID").Select(data =>
			{
				var mod = new EbayOrdersListItemModel(data);
				var entity = _dataService.AddOrder(data);

				mod.Items = mod.Items.Select(x =>
				{
					var ent = entity.OrderItems.Cast<EbayOrderItem>().FirstOrDefault(y => y.LegacyItemId == x.LegacyItemId && y.LineItemId == x.LineItemId && y.LegacyVariationId == x.LegacyVariationId);

					if (ent == null)
					{
						return x;
					}

					x.OrderItemId = ent.Id;

					if (!string.IsNullOrEmpty(ent.Image))
					{
						x.Image = ent.Image;
						x.Character = ent.CharacterName;
						return x;
					}

					var itemFromApi = GetItem(x.LegacyItemId, x.LegacyVariationId);
					x.Image = itemFromApi.Image;
					x.Character = itemFromApi.Character;

					return x;
				});

				entity = _dataService.AddOrder(data);

				return mod;
			}).Where(x => x.OrderDate > DateTime.Now.AddDays(-30)).OrderByDescending(x => x.OrderDate).ToList();


			var model = new EbayOrdersListModel(items, 0, 1000);

			return model;
		}

		/// <summary>
		/// Gets an image and a character name for an item. SKU doesn't actually seem to exist in the API
		/// </summary>
		/// <param name="legacyItemId"></param>
		/// <param name="legacyVariationId"></param>
		/// <returns></returns>
		public GetItemModel GetItem(string legacyItemId, string legacyVariationId)
		{
			if (string.IsNullOrEmpty(legacyVariationId))
			{
				legacyVariationId = "0";
			}

			string response;
			try
			{
				response = _service.GetRequest($"https://api.ebay.com/buy/browse/v1/item/v1|{legacyItemId}|{legacyVariationId}");
			}
			catch (ApiException)
			{
				return new GetItemModel
				{
					Character = "",
					Image = ""
				};
			}

			var obj = JsonConvert.DeserializeObject<GetItemResponse>(response);

			var character = "";
			var sku = "";
			foreach (var aspect in obj.localizedAspects)
			{
				if (aspect.name == "Character")
				{
					character = aspect.value;
				}
				else if (aspect.name == "SKU")
				{
					sku = aspect.value;
				}
			}

			var model = new GetItemModel
			{
				Character = character,
				Image = obj.image.imageUrl,
				SKU = sku
			};

			_dataService.UpdateOrderItemsByLegacyId(legacyItemId, legacyVariationId, model);

			return model;


		}
	}
}
