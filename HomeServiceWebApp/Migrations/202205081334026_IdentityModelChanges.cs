namespace HomeServiceWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IdentityModelChanges : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "DOB", c => c.DateTime());
            AlterColumn("dbo.AspNetUsers", "Age", c => c.Int());
            AlterColumn("dbo.AspNetUsers", "Gender", c => c.Int());

            Sql(@"
            INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'b80b6866-7f69-41ed-a1d9-4029a8a64a2a', N'Admin')
            INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'5ddce0cc-207d-4874-951b-77195eaa7006', N'User')
            INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'488b9853-db96-479a-8360-73c66f7cfadc', N'Vendor')
            ");
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "Gender", c => c.Int(nullable: false));
            AlterColumn("dbo.AspNetUsers", "Age", c => c.Int(nullable: false));
            AlterColumn("dbo.AspNetUsers", "DOB", c => c.DateTime(nullable: false));
        }
    }
}
