namespace GameStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2017064024 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pegi", "Priority", c => c.Int(nullable: false));
            AddColumn("dbo.Pegi", "IsAgeRating", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Pegi", "IsAgeRating");
            DropColumn("dbo.Pegi", "Priority");
        }
    }
}
