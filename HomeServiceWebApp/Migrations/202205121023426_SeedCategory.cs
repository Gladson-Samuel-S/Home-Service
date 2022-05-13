namespace HomeServiceWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedCategory : DbMigration
    {
        public override void Up()
        {
            Sql(@"
                INSERT INTO [dbo].[Categories] ([Name]) VALUES ('Appliance Repair')
                INSERT INTO [dbo].[Categories] ([Name]) VALUES ('Cleaning and Disinfection')
                INSERT INTO [dbo].[Categories] ([Name]) VALUES ('Salon & Massage')
            ");
        }
        
        public override void Down()
        {
        }
    }
}
