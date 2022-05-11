namespace HomeServiceWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedVendorContact : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VendorContacts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Address = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.AspNetUsers", "Vendor_Id", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "Vendor_Id");
            AddForeignKey("dbo.AspNetUsers", "Vendor_Id", "dbo.VendorContacts", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "Vendor_Id", "dbo.VendorContacts");
            DropIndex("dbo.AspNetUsers", new[] { "Vendor_Id" });
            DropColumn("dbo.AspNetUsers", "Vendor_Id");
            DropTable("dbo.VendorContacts");
        }
    }
}
