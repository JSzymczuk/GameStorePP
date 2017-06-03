namespace GameStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _201706012 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CategoryProducts", "Category_Id", "dbo.Categories");
            DropForeignKey("dbo.CategoryProducts", "Product_Id", "dbo.Products");
            DropIndex("dbo.CategoryProducts", new[] { "Category_Id" });
            DropIndex("dbo.CategoryProducts", new[] { "Product_Id" });
            DropTable("dbo.Categories");
            DropTable("dbo.CategoryProducts");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.CategoryProducts",
                c => new
                    {
                        Category_Id = c.Int(nullable: false),
                        Product_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Category_Id, t.Product_Id });
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.CategoryProducts", "Product_Id");
            CreateIndex("dbo.CategoryProducts", "Category_Id");
            AddForeignKey("dbo.CategoryProducts", "Product_Id", "dbo.Products", "Id", cascadeDelete: true);
            AddForeignKey("dbo.CategoryProducts", "Category_Id", "dbo.Categories", "Id", cascadeDelete: true);
        }
    }
}
