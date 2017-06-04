namespace GameStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _201706040 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Requirements", "Product_Id", "dbo.Products");
            DropIndex("dbo.Requirements", new[] { "Product_Id" });
            AddColumn("dbo.Products", "State", c => c.Int(nullable: false));
            AddColumn("dbo.Products", "AddedById", c => c.String(maxLength: 128));
            AddColumn("dbo.Products", "DateEdited", c => c.DateTime());
            AddColumn("dbo.Products", "EditedById", c => c.String(maxLength: 128));
            AddColumn("dbo.Products", "DateDeleted", c => c.DateTime());
            AddColumn("dbo.Products", "DeletedById", c => c.String(maxLength: 128));
            CreateIndex("dbo.Products", "AddedById");
            CreateIndex("dbo.Products", "EditedById");
            CreateIndex("dbo.Products", "DeletedById");
            AddForeignKey("dbo.Products", "AddedById", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Products", "DeletedById", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Products", "EditedById", "dbo.AspNetUsers", "Id");
            DropColumn("dbo.Products", "IsVisible");
            DropColumn("dbo.Requirements", "Product_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Requirements", "Product_Id", c => c.Int());
            AddColumn("dbo.Products", "IsVisible", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.Products", "EditedById", "dbo.AspNetUsers");
            DropForeignKey("dbo.Products", "DeletedById", "dbo.AspNetUsers");
            DropForeignKey("dbo.Products", "AddedById", "dbo.AspNetUsers");
            DropIndex("dbo.Products", new[] { "DeletedById" });
            DropIndex("dbo.Products", new[] { "EditedById" });
            DropIndex("dbo.Products", new[] { "AddedById" });
            DropColumn("dbo.Products", "DeletedById");
            DropColumn("dbo.Products", "DateDeleted");
            DropColumn("dbo.Products", "EditedById");
            DropColumn("dbo.Products", "DateEdited");
            DropColumn("dbo.Products", "AddedById");
            DropColumn("dbo.Products", "State");
            CreateIndex("dbo.Requirements", "Product_Id");
            AddForeignKey("dbo.Requirements", "Product_Id", "dbo.Products", "Id");
        }
    }
}
