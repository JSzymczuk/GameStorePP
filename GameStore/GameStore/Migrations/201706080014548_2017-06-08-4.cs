namespace GameStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _201706084 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderPositions", "OrderId", c => c.Guid(nullable: false));
            AddColumn("dbo.OrderStatusChanges", "OrderId", c => c.Guid(nullable: false));
            CreateIndex("dbo.OrderStatusChanges", "OrderId");
            CreateIndex("dbo.OrderPositions", "OrderId");
            AddForeignKey("dbo.OrderStatusChanges", "OrderId", "dbo.Orders", "Id", cascadeDelete: true);
            AddForeignKey("dbo.OrderPositions", "OrderId", "dbo.Orders", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderPositions", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.OrderStatusChanges", "OrderId", "dbo.Orders");
            DropIndex("dbo.OrderPositions", new[] { "OrderId" });
            DropIndex("dbo.OrderStatusChanges", new[] { "OrderId" });
            DropColumn("dbo.OrderStatusChanges", "OrderId");
            DropColumn("dbo.OrderPositions", "OrderId");
        }
    }
}
