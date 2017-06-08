namespace GameStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migration201706070 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Platforms", "ShortName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Platforms", "ShortName");
        }
    }
}
