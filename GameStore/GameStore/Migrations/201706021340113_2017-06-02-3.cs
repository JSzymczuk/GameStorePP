namespace GameStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _201706023 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "ReleaseDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "ReleaseDate", c => c.DateTime(nullable: false));
        }
    }
}
