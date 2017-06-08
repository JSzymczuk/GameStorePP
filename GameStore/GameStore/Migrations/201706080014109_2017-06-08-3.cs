namespace GameStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _201706083 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Orders", "Address_Id", "dbo.Addresses");
            DropIndex("dbo.Orders", new[] { "Address_Id" });
            RenameColumn(table: "dbo.Orders", name: "Address_Id", newName: "AddressId");
            RenameColumn(table: "dbo.Orders", name: "Client_Id", newName: "ClientId");
            RenameIndex(table: "dbo.Orders", name: "IX_Client_Id", newName: "IX_ClientId");
            AlterColumn("dbo.Orders", "AddressId", c => c.Int(nullable: false));
            CreateIndex("dbo.Orders", "AddressId");
            AddForeignKey("dbo.Orders", "AddressId", "dbo.Addresses", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "AddressId", "dbo.Addresses");
            DropIndex("dbo.Orders", new[] { "AddressId" });
            AlterColumn("dbo.Orders", "AddressId", c => c.Int());
            RenameIndex(table: "dbo.Orders", name: "IX_ClientId", newName: "IX_Client_Id");
            RenameColumn(table: "dbo.Orders", name: "ClientId", newName: "Client_Id");
            RenameColumn(table: "dbo.Orders", name: "AddressId", newName: "Address_Id");
            CreateIndex("dbo.Orders", "Address_Id");
            AddForeignKey("dbo.Orders", "Address_Id", "dbo.Addresses", "Id");
        }
    }
}
