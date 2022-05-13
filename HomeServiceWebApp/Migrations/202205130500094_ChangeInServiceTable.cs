namespace HomeServiceWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeInServiceTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Services", "ServiceName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Services", "ServiceName");
        }
    }
}
