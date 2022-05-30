namespace HomeServiceWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedDiscount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Discount", c => c.Double(nullable: false));
            AddColumn("dbo.AspNetUsers", "IsDiscount", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "IsDiscount");
            DropColumn("dbo.AspNetUsers", "Discount");
        }
    }
}
