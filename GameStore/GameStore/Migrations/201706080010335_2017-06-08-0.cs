namespace GameStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _201706080 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Orders", "AddressId", "dbo.Addresses");
            DropForeignKey("dbo.Orders", "ClientId", "dbo.AspNetUsers");
            DropForeignKey("dbo.OrderStatusChanges", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.OrderPositions", "OrderId", "dbo.Orders");
            DropIndex("dbo.Orders", new[] { "ClientId" });
            DropIndex("dbo.Orders", new[] { "AddressId" });
            DropIndex("dbo.OrderStatusChanges", new[] { "OrderId" });
            DropIndex("dbo.OrderPositions", new[] { "OrderId" });
            DropColumn("dbo.OrderStatusChanges", "OrderId");
            DropColumn("dbo.OrderPositions", "OrderId");
            DropTable("dbo.Orders");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClientId = c.String(maxLength: 128),
                        AddressId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.OrderPositions", "OrderId", c => c.Int(nullable: false));
            AddColumn("dbo.OrderStatusChanges", "OrderId", c => c.Int(nullable: false));
            CreateIndex("dbo.OrderPositions", "OrderId");
            CreateIndex("dbo.OrderStatusChanges", "OrderId");
            CreateIndex("dbo.Orders", "AddressId");
            CreateIndex("dbo.Orders", "ClientId");
            AddForeignKey("dbo.OrderPositions", "OrderId", "dbo.Orders", "Id", cascadeDelete: true);
            AddForeignKey("dbo.OrderStatusChanges", "OrderId", "dbo.Orders", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Orders", "ClientId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Orders", "AddressId", "dbo.Addresses", "Id", cascadeDelete: true);
        }
    }
}
