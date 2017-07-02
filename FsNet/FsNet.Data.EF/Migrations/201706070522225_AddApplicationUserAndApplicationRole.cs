namespace FsNet.Data.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddApplicationUserAndApplicationRole : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Role", "CreationDate", c => c.DateTime());
            AddColumn("dbo.Role", "ModifiedDate", c => c.DateTime());
            AddColumn("dbo.Role", "CreatedBy", c => c.String());
            AddColumn("dbo.Role", "ModifiedBy", c => c.String());
            AddColumn("dbo.User", "MobileNumber", c => c.String(maxLength: 20));
            AddColumn("dbo.User", "CountryPrefix", c => c.String(maxLength: 4));
            AddColumn("dbo.User", "CreationDate", c => c.DateTime());
            AddColumn("dbo.User", "ModifiedDate", c => c.DateTime());
            AddColumn("dbo.User", "CreatedBy", c => c.String(maxLength: 35));
            AddColumn("dbo.User", "ModifiedBy", c => c.String(maxLength: 35));
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "ModifiedBy");
            DropColumn("dbo.User", "CreatedBy");
            DropColumn("dbo.User", "ModifiedDate");
            DropColumn("dbo.User", "CreationDate");
            DropColumn("dbo.User", "CountryPrefix");
            DropColumn("dbo.User", "MobileNumber");
            DropColumn("dbo.Role", "ModifiedBy");
            DropColumn("dbo.Role", "CreatedBy");
            DropColumn("dbo.Role", "ModifiedDate");
            DropColumn("dbo.Role", "CreationDate");
        }
    }
}
