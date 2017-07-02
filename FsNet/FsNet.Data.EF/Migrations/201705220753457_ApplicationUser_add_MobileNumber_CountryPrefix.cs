namespace FsNet.Data.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApplicationUserAddMobileNumberCountryPrefix : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.AspNetUsers", "MobileNumber", c => c.String());
            //AddColumn("dbo.AspNetUsers", "CountryPrefix", c => c.String());
        }
        
        public override void Down()
        {
            //DropColumn("dbo.AspNetUsers", "CountryPrefix");
            //DropColumn("dbo.AspNetUsers", "MobileNumber");
        }
    }
}
