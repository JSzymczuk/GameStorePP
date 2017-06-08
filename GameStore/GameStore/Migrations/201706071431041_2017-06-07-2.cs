namespace GameStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _201706072 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.OrderPositions", "UnitPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.OrderPositions", "UnitPrice", c => c.Double(nullable: false));
        }
    }
}
