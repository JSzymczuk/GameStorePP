namespace GameStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _201706082 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "Address_Id", c => c.Int());
            AddColumn("dbo.Orders", "Client_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Orders", "Address_Id");
            CreateIndex("dbo.Orders", "Client_Id");
            AddForeignKey("dbo.Orders", "Address_Id", "dbo.Addresses", "Id");
            AddForeignKey("dbo.Orders", "Client_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "Client_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Orders", "Address_Id", "dbo.Addresses");
            DropIndex("dbo.Orders", new[] { "Client_Id" });
            DropIndex("dbo.Orders", new[] { "Address_Id" });
            DropColumn("dbo.Orders", "Client_Id");
            DropColumn("dbo.Orders", "Address_Id");
        }
    }
}
