namespace GameStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _201706085 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OrderPositions", "OrderId", "dbo.Orders");
            DropIndex("dbo.OrderPositions", new[] { "OrderId" });
            AlterColumn("dbo.OrderPositions", "OrderId", c => c.Guid());
            CreateIndex("dbo.OrderPositions", "OrderId");
            AddForeignKey("dbo.OrderPositions", "OrderId", "dbo.Orders", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderPositions", "OrderId", "dbo.Orders");
            DropIndex("dbo.OrderPositions", new[] { "OrderId" });
            AlterColumn("dbo.OrderPositions", "OrderId", c => c.Guid(nullable: false));
            CreateIndex("dbo.OrderPositions", "OrderId");
            AddForeignKey("dbo.OrderPositions", "OrderId", "dbo.Orders", "Id", cascadeDelete: true);
        }
    }
}
