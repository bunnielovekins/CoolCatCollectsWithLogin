﻿using CoolCatCollects.Data.Entities.Purchases;
using CoolCatCollects.Data.Repositories;
using CoolCatCollects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoolCatCollects.Services
{
	public class UsedPurchaseService : IDisposable, IUsedPurchaseService
	{
		public IBaseRepository<UsedPurchase> _repo;
		public IBaseRepository<UsedPurchaseWeight> _weightRepo;
		public IBaseRepository<UsedPurchaseBLUpload> _blUploadRepo;

		public UsedPurchaseService(IBaseRepository<UsedPurchase> repo, IBaseRepository<UsedPurchaseWeight> weightRepo, IBaseRepository<UsedPurchaseBLUpload> blUploadRepo)
		{
			_weightRepo = weightRepo;
			_blUploadRepo = blUploadRepo;
			_repo = repo;
		}

		public async Task<IEnumerable<UsedPurchaseModel>> GetAll()
		{
			var UsedPurchases = await _repo.FindAllAsync();

			return UsedPurchases.Select(ToModel).OrderByDescending(x => x.Date);
		}

		public async Task<UsedPurchaseModel> Find(int id)
		{
			var usedPurchase = await _repo.FindOneAsync(id);

			return ToModel(usedPurchase);
		}

		public async Task<UsedPurchaseBLUploadModel> FindBLUpload(int id)
		{
			var upload = await _blUploadRepo.FindOneAsync(id);

			return ToModel(upload);
		}

		public async Task Add(UsedPurchaseModel model)
		{
			var usedPurchase = new UsedPurchase
			{
				Id = model.Id,
				Date = model.Date,
				Source = model.Source,
				SourceUsername = model.SourceUsername,
				OrderNumber = model.OrderNumber,
				Price = model.Price,
				PaymentMethod = model.PaymentMethod,
				Receipt = model.Receipt,
				DistanceTravelled = model.DistanceTravelled,
				Location = model.Location,
				Postage = model.Postage,
				Weight = model.Weight,
				PricePerKilo = model.PricePerKilo,
				CompleteSets = model.CompleteSets,
				Notes = model.Notes
			};

			await _repo.AddAsync(usedPurchase);
		}

		public async Task Edit(UsedPurchaseModel model)
		{
			var usedPurchase = await _repo.FindOneAsync(model.Id);

			usedPurchase.Id = model.Id;
			usedPurchase.Date = model.Date;
			usedPurchase.Source = model.Source;
			usedPurchase.SourceUsername = model.SourceUsername;
			usedPurchase.OrderNumber = model.OrderNumber;
			usedPurchase.Price = model.Price;
			usedPurchase.PaymentMethod = model.PaymentMethod;
			usedPurchase.Receipt = model.Receipt;
			usedPurchase.DistanceTravelled = model.DistanceTravelled;
			usedPurchase.Location = model.Location;
			usedPurchase.Postage = model.Postage;
			usedPurchase.Weight = model.Weight;
			usedPurchase.PricePerKilo = model.PricePerKilo;
			usedPurchase.CompleteSets = model.CompleteSets;
			usedPurchase.Notes = model.Notes;

			await _repo.UpdateAsync(usedPurchase);
		}

		public async Task Delete(int id)
		{
			var usedPurchase = await _repo.FindOneAsync(id);

			await _blUploadRepo.RemoveManyAsync(usedPurchase.BLUploads);
			await _weightRepo.RemoveManyAsync(usedPurchase.Weights);

			await _repo.RemoveAsync(usedPurchase);
		}



		/// <summary>
		/// Gets a used purchase with the associated weights
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public async Task<UsedPurchaseModel> GetPurchaseWithContents(int id)
		{
			var purchase = await _repo.FindOneAsync(id);

			var model = ToModel(purchase);

			model.Weights = purchase.Weights.Select(ToModel);
			model.BLUploads = purchase.BLUploads.Select(ToModel);

			return model;
		}

		/// <summary>
		/// Updates the list of weights for a purchase
		/// </summary>
		/// <param name="id"></param>
		/// <param name="weights"></param>
		/// <returns></returns>
		public async Task UpdateWeights(int id, IEnumerable<UsedPurchaseWeightModel> weights)
		{
			if (weights == null)
			{
				weights = new List<UsedPurchaseWeightModel>();
			}

			var purchase = await _repo.FindOneAsync(id);

			var existingWeights = purchase.Weights;
			var existingIds = existingWeights.Select(x => x.Id);

			var toAdd = weights.Where(x => !existingIds.Contains(x.Id) && !x.Delete).ToList();
			var toUpdate = weights.Where(x => existingIds.Contains(x.Id) && !x.Delete).ToList();
			var toDelete = weights.Where(x => x.Delete).ToList();

			// Add
			foreach (var wt in toAdd.Select(x => ToEntity(x, purchase)))
			{
				await _weightRepo.AddAsync(wt);
			}

			// Update
			foreach (var wt in toUpdate)
			{
				var entity = await _weightRepo.FindOneAsync(wt.Id);

				entity.Colour = wt.Colour;
				entity.Weight = wt.Weight;

				await _weightRepo.UpdateAsync(entity);
			}

			// Delete
			await _weightRepo.RemoveManyAsync(toDelete.Select(x => _weightRepo.FindOne(x.Id)));
		}

		public async Task AddBLUpload(UsedPurchaseBLUploadModel model)
		{
			var entity = ToEntity(model);

			await _blUploadRepo.AddAsync(entity);
		}

		public async Task EditBLUpload(UsedPurchaseBLUploadModel model)
		{
			var entity = ToEntity(model);

			await _blUploadRepo.UpdateAsync(entity);
		}

		public async Task DeleteBLUpload(int id)
		{
			var entity = await _blUploadRepo.FindOneAsync(id);

			await _blUploadRepo.RemoveAsync(entity);
		}

		#region Mappings

		private UsedPurchaseModel ToModel(UsedPurchase usedPurchase)
		{
			return new UsedPurchaseModel
			{
				Id = usedPurchase.Id,
				Date = usedPurchase.Date,
				Source = usedPurchase.Source,
				SourceUsername = usedPurchase.SourceUsername,
				OrderNumber = usedPurchase.OrderNumber,
				Price = usedPurchase.Price,
				PaymentMethod = usedPurchase.PaymentMethod,
				Receipt = usedPurchase.Receipt,
				DistanceTravelled = usedPurchase.DistanceTravelled,
				Location = usedPurchase.Location,
				Postage = usedPurchase.Postage,
				Weight = usedPurchase.Weight,
				PricePerKilo = usedPurchase.PricePerKilo,
				CompleteSets = usedPurchase.CompleteSets,
				Notes = usedPurchase.Notes,
				TotalBundleWeight = usedPurchase.Weights.Sum(x => x.Weight / 1000)
			};
		}

		private UsedPurchaseWeightModel ToModel(UsedPurchaseWeight wt)
		{
			return new UsedPurchaseWeightModel
			{
				Id = wt.Id,
				Colour = wt.Colour,
				Weight = wt.Weight,
				UsedPurchaseId = wt.UsedPurchase.Id
			};
		}

		private UsedPurchaseWeight ToEntity(UsedPurchaseWeightModel wt, UsedPurchase purchase)
		{
			return new UsedPurchaseWeight
			{
				Id = wt.Id,
				Colour = wt.Colour,
				Weight = wt.Weight,
				UsedPurchase = purchase
			};
		}

		private UsedPurchaseBLUploadModel ToModel(UsedPurchaseBLUpload upload)
		{
			return new UsedPurchaseBLUploadModel
			{
				Id = upload.Id,
				Date = upload.Date,
				Parts = upload.Parts,
				Lots = upload.Lots,
				Value = upload.Value,
				Notes = upload.Notes,
				UsedPurchaseId = upload.UsedPurchase.Id
			};
		}

		private UsedPurchaseBLUpload ToEntity(UsedPurchaseBLUploadModel model)
		{
			return new UsedPurchaseBLUpload
			{
				Id = model.Id,
				Date = model.Date,
				Parts = model.Parts,
				Lots = model.Lots,
				Value = model.Value,
				Notes = model.Notes,
				UsedPurchase = _repo.FindOne(model.UsedPurchaseId)
			};
		}

		#endregion

		public void Dispose()
		{
			_repo.Dispose();
		}
	}
}
