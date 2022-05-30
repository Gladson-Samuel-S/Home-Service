namespace HomeServiceWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedDiscount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Services", "Discount", c => c.Double(nullable: false));
            AddColumn("dbo.Services", "IsDiscount", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Services", "IsDiscount");
            DropColumn("dbo.Services", "Discount");
        }
    }
}
