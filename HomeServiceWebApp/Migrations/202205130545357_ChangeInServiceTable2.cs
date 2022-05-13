namespace HomeServiceWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeInServiceTable2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Services", "Price", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Services", "Price");
        }
    }
}
