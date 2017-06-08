namespace GameStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _201706071 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "DefaultAddressId", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "DefaultAddressId", c => c.Int(nullable: false));
        }
    }
}
