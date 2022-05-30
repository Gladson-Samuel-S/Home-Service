namespace HomeServiceWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteDiscountFromServiceTable : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Services", "Discount");
            DropColumn("dbo.Services", "IsDiscount");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Services", "IsDiscount", c => c.Boolean(nullable: false));
            AddColumn("dbo.Services", "Discount", c => c.Double(nullable: false));
        }
    }
}
