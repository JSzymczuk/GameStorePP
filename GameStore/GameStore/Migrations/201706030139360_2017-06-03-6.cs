namespace GameStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _201706036 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Requirements", "ProductId", "dbo.Products");
            DropForeignKey("dbo.Products", "MinimumRequirements_ProductId", "dbo.Requirements");
            DropForeignKey("dbo.Products", "RecommendedRequirements_ProductId", "dbo.Requirements");
            DropIndex("dbo.Products", new[] { "MinimumRequirements_ProductId" });
            DropIndex("dbo.Products", new[] { "RecommendedRequirements_ProductId" });
            DropColumn("dbo.Products", "MinimumRequirementsId");
            DropColumn("dbo.Products", "RecommendedRequirementsId");
            RenameColumn(table: "dbo.Products", name: "MinimumRequirements_ProductId", newName: "MinimumRequirementsId");
            RenameColumn(table: "dbo.Products", name: "RecommendedRequirements_ProductId", newName: "RecommendedRequirementsId");
            DropPrimaryKey("dbo.Requirements");
            AddColumn("dbo.Requirements", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Products", "MinimumRequirementsId", c => c.Int());
            AlterColumn("dbo.Products", "RecommendedRequirementsId", c => c.Int());
            AddPrimaryKey("dbo.Requirements", "Id");
            CreateIndex("dbo.Products", "MinimumRequirementsId");
            CreateIndex("dbo.Products", "RecommendedRequirementsId");
            AddForeignKey("dbo.Requirements", "ProductId", "dbo.Products", "Id", cascadeDelete: false);
            AddForeignKey("dbo.Products", "MinimumRequirementsId", "dbo.Requirements", "Id");
            AddForeignKey("dbo.Products", "RecommendedRequirementsId", "dbo.Requirements", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "RecommendedRequirementsId", "dbo.Requirements");
            DropForeignKey("dbo.Products", "MinimumRequirementsId", "dbo.Requirements");
            DropForeignKey("dbo.Requirements", "ProductId", "dbo.Products");
            DropIndex("dbo.Products", new[] { "RecommendedRequirementsId" });
            DropIndex("dbo.Products", new[] { "MinimumRequirementsId" });
            DropPrimaryKey("dbo.Requirements");
            AlterColumn("dbo.Products", "RecommendedRequirementsId", c => c.Int(nullable: false));
            AlterColumn("dbo.Products", "MinimumRequirementsId", c => c.Int(nullable: false));
            DropColumn("dbo.Requirements", "Id");
            AddPrimaryKey("dbo.Requirements", "ProductId");
            RenameColumn(table: "dbo.Products", name: "RecommendedRequirementsId", newName: "RecommendedRequirements_ProductId");
            RenameColumn(table: "dbo.Products", name: "MinimumRequirementsId", newName: "MinimumRequirements_ProductId");
            AddColumn("dbo.Products", "RecommendedRequirementsId", c => c.Int(nullable: false));
            AddColumn("dbo.Products", "MinimumRequirementsId", c => c.Int(nullable: false));
            CreateIndex("dbo.Products", "RecommendedRequirements_ProductId");
            CreateIndex("dbo.Products", "MinimumRequirements_ProductId");
            AddForeignKey("dbo.Products", "RecommendedRequirements_ProductId", "dbo.Requirements", "ProductId");
            AddForeignKey("dbo.Products", "MinimumRequirements_ProductId", "dbo.Requirements", "ProductId");
            AddForeignKey("dbo.Requirements", "ProductId", "dbo.Products", "Id");
        }
    }
}
