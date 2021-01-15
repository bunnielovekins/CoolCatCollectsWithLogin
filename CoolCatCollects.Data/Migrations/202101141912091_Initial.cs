namespace CoolCatCollects.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrderItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Quantity = c.Int(nullable: false),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 4),
                        LineItemId = c.String(),
                        LegacyItemId = c.String(),
                        LegacyVariationId = c.String(),
                        SKU = c.String(),
                        Image = c.String(),
                        CharacterName = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        Order_Id = c.Int(),
                        Part_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.Order_Id)
                .ForeignKey("dbo.PartInventories", t => t.Part_Id)
                .Index(t => t.Order_Id)
                .Index(t => t.Part_Id);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderId = c.String(),
                        OrderDate = c.DateTime(nullable: false),
                        BuyerName = c.String(),
                        BuyerEmail = c.String(),
                        Subtotal = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Shipping = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Deductions = c.Decimal(nullable: false, precision: 18, scale: 4),
                        ExtraCosts = c.Decimal(nullable: false, precision: 18, scale: 4),
                        GrandTotal = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Tax = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.Int(nullable: false),
                        TotalCount = c.Int(),
                        UniqueCount = c.Int(),
                        Weight = c.String(),
                        DriveThruSent = c.Boolean(),
                        ShippingMethod = c.String(),
                        BuyerRealName = c.String(),
                        LegacyOrderId = c.String(),
                        SalesRecordReference = c.String(),
                        BuyerUsername = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PartInventories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InventoryId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        MyPrice = c.Decimal(nullable: false, precision: 18, scale: 4),
                        ColourId = c.Int(nullable: false),
                        ColourName = c.String(),
                        Condition = c.String(),
                        Location = c.String(),
                        Image = c.String(),
                        Description = c.String(),
                        Notes = c.String(),
                        LastUpdated = c.DateTime(nullable: false),
                        Part_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Parts", t => t.Part_Id, cascadeDelete: true)
                .Index(t => t.Part_Id);
            
            CreateTable(
                "dbo.PartInventoryLocationHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Location = c.String(),
                        PartInventory_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PartInventories", t => t.PartInventory_Id, cascadeDelete: true)
                .Index(t => t.PartInventory_Id);
            
            CreateTable(
                "dbo.Parts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Number = c.String(),
                        ItemType = c.String(),
                        Name = c.String(),
                        CategoryId = c.Int(nullable: false),
                        ImageUrl = c.String(),
                        ThumbnailUrl = c.String(),
                        Weight = c.String(),
                        Description = c.String(),
                        LastUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PartPriceInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        AveragePrice = c.Decimal(nullable: false, precision: 18, scale: 4),
                        AveragePriceLocation = c.String(),
                        LastUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PartInventories", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Colours",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ColourId = c.Int(nullable: false),
                        ColourCode = c.String(),
                        ColourType = c.String(),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Expenses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        TaxCategory = c.String(),
                        Category = c.String(),
                        Source = c.String(),
                        ExpenditureType = c.String(),
                        OrderNumber = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Postage = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Receipt = c.Boolean(nullable: false),
                        Notes = c.String(),
                        Item = c.String(),
                        Quantity = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Infoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InventoryLastUpdated = c.DateTime(nullable: false),
                        OrdersLastUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Logs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Title = c.String(),
                        Note = c.String(),
                        FurtherNote = c.String(),
                        Category = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.NewPurchases",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        SetNumber = c.String(),
                        SetName = c.String(),
                        Theme = c.String(),
                        Promotions = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Quantity = c.Int(nullable: false),
                        Parts = c.Int(nullable: false),
                        TotalParts = c.Int(nullable: false),
                        MinifigureValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PriceToPartOutRatio = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Source = c.String(),
                        PaymentMethod = c.String(),
                        AveragePartOutValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MyPartOutValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExpectedGrossProfit = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExpectedNetProfit = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.String(),
                        SellingNotes = c.String(),
                        Notes = c.String(),
                        Receipt = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UsedPurchaseBLUploads",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Parts = c.Int(nullable: false),
                        Lots = c.Int(nullable: false),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Notes = c.String(),
                        UsedPurchase_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UsedPurchases", t => t.UsedPurchase_Id)
                .Index(t => t.UsedPurchase_Id);
            
            CreateTable(
                "dbo.UsedPurchases",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Source = c.String(),
                        SourceUsername = c.String(),
                        OrderNumber = c.String(),
                        Price = c.String(),
                        PaymentMethod = c.String(),
                        Receipt = c.Boolean(nullable: false),
                        DistanceTravelled = c.String(),
                        Location = c.String(),
                        Postage = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Weight = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PricePerKilo = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CompleteSets = c.Boolean(nullable: false),
                        Notes = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UsedPurchaseWeights",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Colour = c.String(),
                        Weight = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UsedPurchase_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UsedPurchases", t => t.UsedPurchase_Id)
                .Index(t => t.UsedPurchase_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UsedPurchaseWeights", "UsedPurchase_Id", "dbo.UsedPurchases");
            DropForeignKey("dbo.UsedPurchaseBLUploads", "UsedPurchase_Id", "dbo.UsedPurchases");
            DropForeignKey("dbo.OrderItems", "Part_Id", "dbo.PartInventories");
            DropForeignKey("dbo.PartPriceInfoes", "Id", "dbo.PartInventories");
            DropForeignKey("dbo.PartInventories", "Part_Id", "dbo.Parts");
            DropForeignKey("dbo.PartInventoryLocationHistories", "PartInventory_Id", "dbo.PartInventories");
            DropForeignKey("dbo.OrderItems", "Order_Id", "dbo.Orders");
            DropIndex("dbo.UsedPurchaseWeights", new[] { "UsedPurchase_Id" });
            DropIndex("dbo.UsedPurchaseBLUploads", new[] { "UsedPurchase_Id" });
            DropIndex("dbo.PartPriceInfoes", new[] { "Id" });
            DropIndex("dbo.PartInventoryLocationHistories", new[] { "PartInventory_Id" });
            DropIndex("dbo.PartInventories", new[] { "Part_Id" });
            DropIndex("dbo.OrderItems", new[] { "Part_Id" });
            DropIndex("dbo.OrderItems", new[] { "Order_Id" });
            DropTable("dbo.UsedPurchaseWeights");
            DropTable("dbo.UsedPurchases");
            DropTable("dbo.UsedPurchaseBLUploads");
            DropTable("dbo.NewPurchases");
            DropTable("dbo.Logs");
            DropTable("dbo.Infoes");
            DropTable("dbo.Expenses");
            DropTable("dbo.Colours");
            DropTable("dbo.PartPriceInfoes");
            DropTable("dbo.Parts");
            DropTable("dbo.PartInventoryLocationHistories");
            DropTable("dbo.PartInventories");
            DropTable("dbo.Orders");
            DropTable("dbo.OrderItems");
        }
    }
}
