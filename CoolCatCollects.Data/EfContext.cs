﻿using CoolCatCollects.Data.Entities;
using CoolCatCollects.Data.Entities.Expenses;
using CoolCatCollects.Data.Entities.Purchases;
using CoolCatCollects.Data.Migrations;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace CoolCatCollects.Data
{
	public class EfContext : IdentityDbContext<ApplicationUser>
	{
		public EfContext() : base("DataContext")
		{
			Database.SetInitializer(new MigrateDatabaseToLatestVersion<EfContext, Configuration>());
		}

		public static EfContext Create()
		{
			return new EfContext();
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			// Setting precision for decimals that come from BrickLink
			modelBuilder.Entity<PartInventory>().Property(x => x.MyPrice).HasPrecision(18, 4);
			modelBuilder.Entity<PartPriceInfo>().Property(x => x.AveragePrice).HasPrecision(18, 4);
			modelBuilder.Entity<Order>().Property(x => x.Subtotal).HasPrecision(18, 4);
			modelBuilder.Entity<Order>().Property(x => x.Shipping).HasPrecision(18, 4);
			modelBuilder.Entity<Order>().Property(x => x.Deductions).HasPrecision(18, 4);
			modelBuilder.Entity<Order>().Property(x => x.ExtraCosts).HasPrecision(18, 4);
			modelBuilder.Entity<Order>().Property(x => x.GrandTotal).HasPrecision(18, 4);
			modelBuilder.Entity<OrderItem>().Property(x => x.UnitPrice).HasPrecision(18, 4);
		}

		public DbSet<Info> Infos { get; set; }
		public DbSet<Part> Parts { get; set; }
		public DbSet<PartInventory> PartInventorys { get; set; }
		public DbSet<PartPriceInfo> PartPriceInfos { get; set; }
		public DbSet<PartInventoryLocationHistory> PartInventoryLocationHistorys { get; set; }
		public DbSet<Colour> Colours { get; set; }
		public DbSet<EbayOrder> EbayOrders { get; set; }
		public DbSet<EbayOrderItem> EbayOrderItems { get; set; }
		public DbSet<BricklinkOrder> BricklinkOrders { get; set; }
		public DbSet<BricklinkOrderItem> BricklinkOrderItems { get; set; }
		public DbSet<Expense> Expenses { get; set; }
		public DbSet<NewPurchase> NewPurchases { get; set; }
		public DbSet<UsedPurchase> UsedPurchases { get; set; }
		public DbSet<UsedPurchaseWeight> UsedPurchaseWeights { get; set; }
		public DbSet<Log> Logs { get; set; }

		public DbSet<UsedPurchaseBLUpload> UsedPurchaseBLUploads { get; set; }
	}
}
