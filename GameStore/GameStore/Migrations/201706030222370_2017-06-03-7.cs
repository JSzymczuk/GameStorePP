namespace GameStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _201706037 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Requirements", "ProductId", "dbo.Products");
            DropIndex("dbo.Requirements", new[] { "ProductId" });
            RenameColumn(table: "dbo.Requirements", name: "ProductId", newName: "Product_Id");
            AlterColumn("dbo.Requirements", "Product_Id", c => c.Int());
            CreateIndex("dbo.Requirements", "Product_Id");
            AddForeignKey("dbo.Requirements", "Product_Id", "dbo.Products", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Requirements", "Product_Id", "dbo.Products");
            DropIndex("dbo.Requirements", new[] { "Product_Id" });
            AlterColumn("dbo.Requirements", "Product_Id", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Requirements", name: "Product_Id", newName: "ProductId");
            CreateIndex("dbo.Requirements", "ProductId");
            AddForeignKey("dbo.Requirements", "ProductId", "dbo.Products", "Id", cascadeDelete: true);
        }
    }
}
