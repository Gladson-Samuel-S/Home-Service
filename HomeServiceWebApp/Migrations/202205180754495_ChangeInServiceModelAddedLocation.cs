namespace HomeServiceWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeInServiceModelAddedLocation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Services", "Location", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Services", "Location");
        }
    }
}
