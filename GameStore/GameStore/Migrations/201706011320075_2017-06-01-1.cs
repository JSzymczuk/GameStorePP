namespace GameStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _201706011 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Pegis", newName: "Pegi");
            CreateTable(
                "dbo.OrderStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.AspNetUsers", "FirstName", c => c.String());
            AddColumn("dbo.AspNetUsers", "LastName", c => c.String());
            AddColumn("dbo.OrderStatusChanges", "StatusId", c => c.Int(nullable: false));
            AddColumn("dbo.Pegi", "Name", c => c.String());
            CreateIndex("dbo.OrderStatusChanges", "StatusId");
            AddForeignKey("dbo.OrderStatusChanges", "StatusId", "dbo.OrderStatus", "Id", cascadeDelete: true);
            DropColumn("dbo.OrderStatusChanges", "Status");
            DropColumn("dbo.Pegi", "ShortName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Pegi", "ShortName", c => c.String());
            AddColumn("dbo.OrderStatusChanges", "Status", c => c.Int(nullable: false));
            DropForeignKey("dbo.OrderStatusChanges", "StatusId", "dbo.OrderStatus");
            DropIndex("dbo.OrderStatusChanges", new[] { "StatusId" });
            DropColumn("dbo.Pegi", "Name");
            DropColumn("dbo.OrderStatusChanges", "StatusId");
            DropColumn("dbo.AspNetUsers", "LastName");
            DropColumn("dbo.AspNetUsers", "FirstName");
            DropTable("dbo.OrderStatus");
            RenameTable(name: "dbo.Pegi", newName: "Pegis");
        }
    }
}
