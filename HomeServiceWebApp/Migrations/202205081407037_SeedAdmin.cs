namespace HomeServiceWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedAdmin : DbMigration
    {
        public override void Up()
        {
            Sql(@"
                INSERT INTO [dbo].[AspNetUsers] ([Id] ,[FirstName] ,[LastName] ,[DOB] ,[Age] ,[Gender] ,[Email] ,[EmailConfirmed] ,[PasswordHash] ,[SecurityStamp] ,[PhoneNumber] ,[PhoneNumberConfirmed] ,[TwoFactorEnabled] ,[LockoutEndDateUtc] ,[LockoutEnabled] ,[AccessFailedCount] ,[UserName]) VALUES ('3d4271ea-1cb1-4357-afd5-01915159065d','Admin','Admin','1990-01-01 00:00:00',NULL,NULL,'admin@gmail.com',0,'AI7OBud5zgMZTJFcDy9anwr0GikWbsHJQyl/UX460KkgnCnnCVA7wywjh3Ct7Zytsw==','2e110e5d-0a78-41aa-83c1-28b315de1549','1234567890',0,0,NULL,1,0,'admin@gmail.com')

                INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES ('3d4271ea-1cb1-4357-afd5-01915159065d', 'b80b6866-7f69-41ed-a1d9-4029a8a64a2a')
            ");
        }
        
        public override void Down()
        {
        }
    }
}
