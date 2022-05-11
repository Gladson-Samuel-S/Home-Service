namespace HomeServiceWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeInVendorContactModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "Vendor_Id", "dbo.VendorContacts");
            DropIndex("dbo.AspNetUsers", new[] { "Vendor_Id" });
            RenameColumn(table: "dbo.AspNetUsers", name: "Vendor_Id", newName: "VendorId");
            AlterColumn("dbo.AspNetUsers", "VendorId", c => c.Int(nullable: true));
            CreateIndex("dbo.AspNetUsers", "VendorId");
            AddForeignKey("dbo.AspNetUsers", "VendorId", "dbo.VendorContacts", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "VendorId", "dbo.VendorContacts");
            DropIndex("dbo.AspNetUsers", new[] { "VendorId" });
            AlterColumn("dbo.AspNetUsers", "VendorId", c => c.Int());
            RenameColumn(table: "dbo.AspNetUsers", name: "VendorId", newName: "Vendor_Id");
            CreateIndex("dbo.AspNetUsers", "Vendor_Id");
            AddForeignKey("dbo.AspNetUsers", "Vendor_Id", "dbo.VendorContacts", "Id");
        }
    }
}
