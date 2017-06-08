namespace GameStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _201706087 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderStatus", "Cancellable", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderStatus", "Cancellable");
        }
    }
}
