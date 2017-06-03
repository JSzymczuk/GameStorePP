namespace GameStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20170601 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        IsDeleted = c.Boolean(nullable: false),
                        Region = c.String(),
                        City = c.String(),
                        Street = c.String(),
                        PostalCode = c.String(),
                        AdditionalInfo = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClientId = c.String(maxLength: 128),
                        AddressId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Addresses", t => t.AddressId, cascadeDelete: false)
                .ForeignKey("dbo.AspNetUsers", t => t.ClientId)
                .Index(t => t.ClientId)
                .Index(t => t.AddressId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        DefaultAddressId = c.Int(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: false)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: false)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: false)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: false)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.OrderStatusChanges",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderId = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: false)
                .Index(t => t.OrderId);
            
            CreateTable(
                "dbo.OrderPositions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        UnitPrice = c.Double(nullable: false),
                        Quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: false)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: false)
                .Index(t => t.OrderId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PlatformId = c.Int(nullable: false),
                        MinimumRequirementsId = c.Int(nullable: false),
                        RecommendedRequirementsId = c.Int(nullable: false),
                        IsVisible = c.Boolean(nullable: false),
                        DateAdded = c.DateTime(nullable: false),
                        Price = c.Double(nullable: false),
                        CoverPath = c.String(),
                        ThumbPath = c.String(),
                        Name = c.String(),
                        Studio = c.String(),
                        ReleaseDate = c.DateTime(nullable: false),
                        Description = c.String(),
                        MinimumRequirements_ProductId = c.Int(),
                        RecommendedRequirements_ProductId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Requirements", t => t.MinimumRequirements_ProductId)
                .ForeignKey("dbo.Platforms", t => t.PlatformId, cascadeDelete: false)
                .ForeignKey("dbo.Requirements", t => t.RecommendedRequirements_ProductId)
                .Index(t => t.PlatformId)
                .Index(t => t.MinimumRequirements_ProductId)
                .Index(t => t.RecommendedRequirements_ProductId);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Requirements",
                c => new
                    {
                        ProductId = c.Int(nullable: false),
                        OS = c.String(),
                        CPU = c.String(),
                        GPU = c.String(),
                        RAM = c.String(),
                        HDD = c.String(),
                        DirectX = c.String(),
                    })
                .PrimaryKey(t => t.ProductId)
                .ForeignKey("dbo.Products", t => t.ProductId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.Pegis",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ShortName = c.String(),
                        Description = c.String(),
                        IconPath = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Platforms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.CategoryProducts",
                c => new
                    {
                        Category_Id = c.Int(nullable: false),
                        Product_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Category_Id, t.Product_Id })
                .ForeignKey("dbo.Categories", t => t.Category_Id, cascadeDelete: false)
                .ForeignKey("dbo.Products", t => t.Product_Id, cascadeDelete: false)
                .Index(t => t.Category_Id)
                .Index(t => t.Product_Id);
            
            CreateTable(
                "dbo.PegiProducts",
                c => new
                    {
                        Pegi_Id = c.Int(nullable: false),
                        Product_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Pegi_Id, t.Product_Id })
                .ForeignKey("dbo.Pegis", t => t.Pegi_Id, cascadeDelete: false)
                .ForeignKey("dbo.Products", t => t.Product_Id, cascadeDelete: false)
                .Index(t => t.Pegi_Id)
                .Index(t => t.Product_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.OrderPositions", "ProductId", "dbo.Products");
            DropForeignKey("dbo.Products", "RecommendedRequirements_ProductId", "dbo.Requirements");
            DropForeignKey("dbo.Products", "PlatformId", "dbo.Platforms");
            DropForeignKey("dbo.PegiProducts", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.PegiProducts", "Pegi_Id", "dbo.Pegis");
            DropForeignKey("dbo.Products", "MinimumRequirements_ProductId", "dbo.Requirements");
            DropForeignKey("dbo.Requirements", "ProductId", "dbo.Products");
            DropForeignKey("dbo.CategoryProducts", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.CategoryProducts", "Category_Id", "dbo.Categories");
            DropForeignKey("dbo.OrderPositions", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.OrderStatusChanges", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Orders", "ClientId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Addresses", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Orders", "AddressId", "dbo.Addresses");
            DropIndex("dbo.PegiProducts", new[] { "Product_Id" });
            DropIndex("dbo.PegiProducts", new[] { "Pegi_Id" });
            DropIndex("dbo.CategoryProducts", new[] { "Product_Id" });
            DropIndex("dbo.CategoryProducts", new[] { "Category_Id" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Requirements", new[] { "ProductId" });
            DropIndex("dbo.Products", new[] { "RecommendedRequirements_ProductId" });
            DropIndex("dbo.Products", new[] { "MinimumRequirements_ProductId" });
            DropIndex("dbo.Products", new[] { "PlatformId" });
            DropIndex("dbo.OrderPositions", new[] { "ProductId" });
            DropIndex("dbo.OrderPositions", new[] { "OrderId" });
            DropIndex("dbo.OrderStatusChanges", new[] { "OrderId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Orders", new[] { "AddressId" });
            DropIndex("dbo.Orders", new[] { "ClientId" });
            DropIndex("dbo.Addresses", new[] { "UserId" });
            DropTable("dbo.PegiProducts");
            DropTable("dbo.CategoryProducts");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Platforms");
            DropTable("dbo.Pegis");
            DropTable("dbo.Requirements");
            DropTable("dbo.Categories");
            DropTable("dbo.Products");
            DropTable("dbo.OrderPositions");
            DropTable("dbo.OrderStatusChanges");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Orders");
            DropTable("dbo.Addresses");
        }
    }
}
