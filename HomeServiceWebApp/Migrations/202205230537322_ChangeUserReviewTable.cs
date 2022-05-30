namespace HomeServiceWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeUserReviewTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserReviews", "ServiceId", c => c.Int(nullable: false));
            CreateIndex("dbo.UserReviews", "ServiceId");
            AddForeignKey("dbo.UserReviews", "ServiceId", "dbo.Services", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserReviews", "ServiceId", "dbo.Services");
            DropIndex("dbo.UserReviews", new[] { "ServiceId" });
            DropColumn("dbo.UserReviews", "ServiceId");
        }
    }
}
