namespace HomeServiceWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateUserReviews : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserReviews", "ServiceId", "dbo.Services");
            DropIndex("dbo.UserReviews", new[] { "ServiceId" });
            DropColumn("dbo.UserReviews", "ServiceId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserReviews", "ServiceId", c => c.Int(nullable: false));
            CreateIndex("dbo.UserReviews", "ServiceId");
            AddForeignKey("dbo.UserReviews", "ServiceId", "dbo.Services", "Id", cascadeDelete: true);
        }
    }
}
