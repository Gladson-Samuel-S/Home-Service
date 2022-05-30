namespace HomeServiceWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreatingUserReviewTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserReviews",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Rating = c.String(nullable: false),
                        Review = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Orders", "UserReviewId", c => c.Int());
            CreateIndex("dbo.Orders", "UserReviewId");
            AddForeignKey("dbo.Orders", "UserReviewId", "dbo.UserReviews", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "UserReviewId", "dbo.UserReviews");
            DropIndex("dbo.Orders", new[] { "UserReviewId" });
            DropColumn("dbo.Orders", "UserReviewId");
            DropTable("dbo.UserReviews");
        }
    }
}
